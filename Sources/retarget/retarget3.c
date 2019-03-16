/* code (c) kometbomb 2007 */
/* use as you like */

#include <SDL/SDL.h>
#include <SDL/SDL_image.h>
#include <math.h>
#include <limits.h>
#include <windows.h>

#define RANGE 4

#if (SDL_BYTEORDER == SDL_BIG_ENDIAN)
    const Uint32 rmask = 0xff000000;
    const Uint32 gmask = 0x00ff0000;
    const Uint32 bmask = 0x0000ff00;
    const Uint32 amask = 0x000000ff;
#else
    const Uint32 rmask = 0x000000ff;
    const Uint32 gmask = 0x0000ff00;
    const Uint32 bmask = 0x00ff0000;
    const Uint32 amask = 0xff000000;
#endif


typedef struct {
	unsigned char r,g,b;
} pixel_t;

void get_pixel(pixel_t *dst,SDL_Surface *surface,int x,int y) {
	Uint8 *p=surface->pixels+surface->pitch*y+x*surface->format->BytesPerPixel;
	dst->r=p[0];
	dst->g=p[1];
	dst->b=p[2];
}

void put_pixel(SDL_Surface *surface,int x,int y,int r, int g, int b) {
	Uint8 *p=surface->pixels+surface->pitch*y+x*surface->format->BytesPerPixel;
	*((Uint8*)p)=r;
	*((Uint8*)p+1)=g;
	*((Uint8*)p+2)=b;
}

void put_block(float *d,int w,int h,int x,int y,float c) {
	int dx,dy;
	for (dy=-8;dy<8;dy++)	
		for (dx=-8;dx<8;dx++) {
			if (dx+x>=0 && dx+x<w	&& dy+y>=0 && dy+y<h) {
				d[x+dx+(y+dy)*w]=c;	
			}
		}
	
}

void eat_pixel(SDL_Surface *surface,float *gradient,int x,int y,const int origwidth,const int axis) {
	if (axis) {
		// the average of pixels
		
		pixel_t p1,p2;
		get_pixel(&p1,surface,x,y);
		get_pixel(&p2,surface,x+1,y);
				
		put_pixel(surface,x,y,(p1.r+p2.r)/2,(p1.g+p2.g)/2,(p1.b+p2.b)/2);
		
		SDL_Rect src={x+2,y,surface->w-x-2,1};
		SDL_Rect dst={x+1,y,0,0};
		SDL_BlitSurface(surface,&src,surface,&dst);
		
		gradient[x+y*origwidth]=(gradient[x+y*origwidth]+gradient[x+1+y*origwidth])/2;
		memcpy(&gradient[x+1+y*origwidth],&gradient[x+2+y*origwidth],sizeof(int)*(origwidth-x-2));

	} else {
		pixel_t p1,p2;
		get_pixel(&p1,surface,y,x);
		get_pixel(&p2,surface,y,x+1);
		
		put_pixel(surface,y,x,(p1.r+p2.r)/2,(p1.g+p2.g)/2,(p1.b+p2.b)/2);
		
		SDL_Rect src={y,x+2,1,surface->h-x-2};
		SDL_Rect dst={y,x+1,0,0};
		SDL_BlitSurface(surface,&src,surface,&dst);
		
		gradient[(x)*origwidth+y]=(gradient[(x)*origwidth+y]+gradient[(x+1)*origwidth+y])/2;
		
		float *ptr=&gradient[(x+1)*origwidth+y];
		float *ptr2=&gradient[(x+2)*origwidth+y];
		int py;
		for (py=x;py<surface->h-x-2;py++) {
			
			*ptr=*ptr2;;
			ptr+=origwidth;
			ptr2+=origwidth;			
		}
	}
}

/* a lame box blur */
void blur(SDL_Surface *s,const int size) {
	int x,y;
	for (y=0;y<s->h;y++) {
		for (x=0;x<s->w;x++) {
			int dx,dy;
			int r=0,g=0,b=0,c=0;
			for (dy=-size;dy<=size;dy++) {
				if (dy+y>=0 && dy+y<s->h) {
					for (dx=-size;dx<=size;dx++) {
						if (dx+x>=0 && dx+x<s->w) {
							pixel_t p;
							get_pixel(&p,s,x+dx,y+dy);
							r+=p.r;
							g+=p.g;
							b+=p.b;
							c++;
						} 
					}	
				} 
			}
			const int ss=c;
			put_pixel(s,x,y,r/ss,g/ss,b/ss);
		}
	}
}

/* a bastardization of the sobel operator */
void build_gradient_map(float *dst,SDL_Surface *src) {
	static const int mx[3][3]={
		{-1,0,1},
		{-2,0,2},
		{-1,0,1}
		};
		
	static const int my[3][3]={
		{-1,-2,-1},
		{ 0,0,0},
		{1,2,1}
		};
		
	int x,y;
	for (y=0;y<src->h;y++) {
		for (x=0;x<src->w;x++) {
			int dx,dy;
			int gx=0,gy=0;
			for (dy=-1;dy<2;dy++) {
				if (dy+y>=0 && dy+y<src->h) {
					for (dx=-1;dx<2;dx++) {
						if (dx+x>=0 && dx+x<src->w) {
							pixel_t p;
							get_pixel(&p,src,x+dx,y+dy);
							const int a=p.r+p.g+p.b;
							gx+=mx[dy+1][dx+1]*a;
							gy+=my[dy+1][dx+1]*a;
						} 
					}	
				} 
			}
			dst[x+y*src->w]=sqrt(gx*gx+gy*gy);
		}
	}
}

/* does the magic - axis == 0 -> vertical, 1 -> horiz */
void shrink_surface(SDL_Surface *surface,float *gradient,int remove,int width,const int axis) {
	int s=remove;
	
	while (s>0) {
		int x;
		float best_weight=INT_MAX;
		int best_route[axis?surface->h:surface->w];
		
		for (x=0;x<width;x++) {
			float weight=0;
			int px=x,y;
			int route[axis?surface->h:surface->w];
			route[0]=px;
			for (y=1;y<(axis?surface->h:surface->w);y++) {
				int dx,bx=0;
				float best=INT_MAX;
				
				for (dx=-RANGE;dx<(RANGE+1);dx++) {
					if (px+dx>=0 && px+dx<((axis?surface->w:surface->h)-1)) {
						int f;
						
						if (axis) {
							f=gradient[px+dx+y*surface->w];
						} else {
							f=gradient[y+(px+dx)*surface->w];
						}
						
						
						
						if (f<best || (f==best && dx==0) ) {
							best=f;
							bx=dx;	
						}
					}
				}
				
				weight+=best;
				
				if (weight>=best_weight) {
					// no point in continuing because the path is already worse than the best path yet
					break;
				}
				
				px+=bx;
				route[y]=px;
			}
			
			if (weight<best_weight) {
				best_weight=weight;
				memcpy(best_route,route,sizeof(route));
			}
		}
		
		int y;
		for (y=0;y<(axis?surface->h:surface->w);y++) {
			eat_pixel(surface,gradient,best_route[y],y,surface->w,axis);	
		}
		
		char str[100];
		sprintf(str,"Resizing... %d%%",100-(s*100)/remove);
		
		SDL_WM_SetCaption(str, NULL);
		
		s--;
		width--;
	}
}

void scale(int w,int h,int *scaled_width,int *scaled_height,SDL_Surface *original,SDL_Surface *scaled,float *gradient,float *orig_gradient) {
	
	/* ok, this would be much better even with global vars but who cares? it works, somewhat. */
	
	if (w==*scaled_width && h==*scaled_height) return;
	
	
	if (w>original->w) w=original->w;
	if (h>original->h) h=original->h;
	
	if (w>*scaled_width || h>*scaled_height) {
		// we need to start over, won't do upscaling
		SDL_BlitSurface(original,NULL,scaled,NULL);
		memcpy(gradient,orig_gradient,sizeof(float)*original->w*original->h);
		*scaled_width=original->w;
		*scaled_height=original->h;
		shrink_surface(scaled,gradient,*scaled_width-w,*scaled_width,1);	
		shrink_surface(scaled,gradient,*scaled_height-h,*scaled_height,0);
		*scaled_width=w;
		*scaled_height=h;	
	} else {
		if (w<*scaled_width) {
			shrink_surface(scaled,gradient,*scaled_width-w,*scaled_width,1);
			*scaled_width=w;
		} 
		if (h<*scaled_height) {
			shrink_surface(scaled,gradient,*scaled_height-h,*scaled_height,0);
			*scaled_height=h;
		} 
	}	
}

#undef main

int main(int argc,char **argv) {
	SDL_Init(SDL_INIT_VIDEO|SDL_INIT_NOPARACHUTE);
	atexit(SDL_Quit);
	
	if (argc<2) {
		MessageBox(0,"Usage: retarget.exe <image> [<targetwidth> <height> [blur]]","Error",0);
		return 1;
	}
	
	int blur_amount=2;
	
	if (argc==5) {
		blur_amount=atoi(argv[4]);	
	}
	
	SDL_Surface *original=IMG_Load(argv[1]);
	
	if (!original) {
		MessageBox(0,"Can't open image","Error",0);
		return 1;
	}
	
	
	SDL_Surface *scaled=SDL_CreateRGBSurface(SDL_SWSURFACE,original->w,original->h,32,rmask, gmask, bmask, amask);
	
	float *orig_gradient=malloc(sizeof(float)*original->w*original->h);
	float *gradient=malloc(sizeof(float)*original->w*original->h);
	
	SDL_BlitSurface(original,NULL,scaled,NULL);
	/* let's use the scaled image as a temp buffer */
	blur(scaled,blur_amount);
	build_gradient_map(orig_gradient,scaled);
	
	SDL_BlitSurface(original,NULL,scaled,NULL);
	
	memcpy(gradient,orig_gradient,sizeof(float)*original->w*original->h);
	
	int w,h;
	int scaled_width=scaled->w;
	int scaled_height=scaled->h;
	
	if (argc>=4) {
		w=atoi(argv[2]);
		h=atoi(argv[3]);
		
		if (w<32) w=32;
		if (h<32) h=32;
		
		printf("Scaling from %dx%d to %dx%d... go have a coffee\n",original->w,original->h,w,h);			
		
		scale(w,h,&scaled_width,&scaled_height,original,scaled,gradient,orig_gradient);
		
	}  else {
		w=original->w;	
		h=original->h;
	}
	
	SDL_Surface *screen=SDL_SetVideoMode(w,h,32,SDL_HWSURFACE|SDL_DOUBLEBUF|SDL_RESIZABLE);
	
	
	int done=0;
	
	while (!done) {
		SDL_Event e;
		while (SDL_PollEvent(&e)) {
			switch (e.type) {
				case SDL_QUIT:
					done=1;
				break;	
				
				case SDL_MOUSEMOTION: 
				case SDL_MOUSEBUTTONDOWN: {
					int x,y;
					int button=SDL_GetMouseState(&x,&y);
					
					if (button&SDL_BUTTON(1)) {
						put_block(gradient,original->w,original->h,x,y,65536);
					} else if (button&SDL_BUTTON(2)) {
						put_block(gradient,original->w,original->h,x,y,0);
					} else if (button&SDL_BUTTON(3)) {
						put_block(gradient,original->w,original->h,x,y,-65536);
					} 
				} break;
				
				case SDL_VIDEORESIZE: {
					int w=e.resize.w;
					int h=e.resize.h;
					if (w<32) w=32;
					if (h<32) h=32;
					
					scale(w,h,&scaled_width,&scaled_height,original,scaled,gradient,orig_gradient);
					
					
					
					screen=SDL_SetVideoMode(w,h,32,SDL_HWSURFACE|SDL_DOUBLEBUF|SDL_RESIZABLE);
				} break;
			}	
			
		}
		
		SDL_BlitSurface(scaled,NULL,screen,NULL);
		
		
		
		/* draw the user painted penalty areas */
		/* to make it faster, eliminate the per pixel x+y*original->w with a pointer inc etc. */
		 
		int x,y;
		for (y=0;y<scaled_height;y++)
		for (x=0;x<scaled_width;x++) {
			if (gradient[x+y*original->w]>=65536) {
				put_pixel(screen,x,y,0,255,0);					
			} else if (gradient[x+y*original->w]<0) {
				put_pixel(screen,x,y,0,0,255);					
			} 
		}
		
		SDL_Flip(screen);
		SDL_Delay(10);
	}
	
	if (MessageBox(0,"Save changes?","Save",MB_YESNO)==IDYES) {
		SDL_SaveBMP(scaled,argv[1]);
	}
	
	SDL_FreeSurface(scaled);
	SDL_FreeSurface(original);
	
	free(orig_gradient);
	free(gradient);
	
	return 0;
}
