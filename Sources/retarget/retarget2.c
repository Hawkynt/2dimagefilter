/* code (c) kometbomb 2007 */
/* use as you like */

#include <SDL/SDL.h>
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

void eat_pixel(SDL_Surface *surface,int x,int y,const int axis) {
	if (axis) {
		SDL_Rect src={x+1,y,surface->w-x-1,1};
		SDL_Rect dst={x,y,0,0};
		SDL_BlitSurface(surface,&src,surface,&dst);
	} else {
		SDL_Rect src={y,x+1,1,surface->h-x-1};
		SDL_Rect dst={y,x,0,0};
		SDL_BlitSurface(surface,&src,surface,&dst);
		
	}
}

/* does the magic - axis == 0 -> vertical, 0 -> horiz */
void shrink_surface(SDL_Surface *surface,int remove,int width,const int axis) {
	int s=remove;
	
	while (s>0) {
		int x;
		Uint64 best_weight=-1;
		int best_route[axis?surface->h:surface->w];
		
		for (x=0;x<width;x++) {
			Uint64 weight=0;
			int px=x,y;
			int route[axis?surface->h:surface->w];
			route[0]=px;
			for (y=1;y<(axis?surface->h:surface->w);y++) {
				int dx,bx=0;
				Uint64 best=-1;
				
				for (dx=-RANGE;dx<(RANGE+1);dx++) {
					if (px+dx>=0 && px+dx<((axis?surface->w:surface->h)-1)) {
						Uint64 f=0;
						pixel_t p1,p2;
						
						if (axis) {
							get_pixel(&p1,surface,px+dx,y);
							get_pixel(&p2,surface,px+dx+1,y);
						} else {
							get_pixel(&p1,surface,y,px+dx);
							get_pixel(&p2,surface,y,px+dx+1);
						}
						
						f+=abs((int)p1.r-(int)p2.r);
						f+=abs((int)p1.g-(int)p2.g);
						f+=abs((int)p1.b-(int)p2.b);
						f=f*f;
						
						if (f<best || (f==best && dx==0) ) {
							best=f;
							bx=dx;	
						}
					}
				}
				
				weight+=best;
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
			eat_pixel(surface,best_route[y],y,axis);	
		}
		
		char str[100];
		sprintf(str,"Resizing... %d%%",100-(s*100)/remove);
		
		SDL_WM_SetCaption(str, NULL);
		
		s--;
		width--;
	}
}

#undef main

int main(int argc,char **argv) {
	SDL_Init(SDL_INIT_VIDEO|SDL_INIT_NOPARACHUTE);
	atexit(SDL_Quit);
	
	if (argc!=2) {
		MessageBox(0,"Usage: retarget.exe <image.bmp>","Error",0);
		return 1;
	}
	
	SDL_Surface *original=SDL_LoadBMP(argv[1]);
	
	if (!original) {
		MessageBox(0,"Can't open image","Error",0);
		return 1;
	}
	
	SDL_Surface *screen=SDL_SetVideoMode(original->w,original->h,32,SDL_HWSURFACE|SDL_DOUBLEBUF|SDL_RESIZABLE);
	SDL_Surface *scaled=SDL_CreateRGBSurface(SDL_SWSURFACE,original->w,original->h,32,rmask, gmask, bmask, amask);
	
	SDL_BlitSurface(original,NULL,scaled,NULL);
	
	int scaled_width=scaled->w;
	int scaled_height=scaled->h;
	int done=0;
	
	while (!done) {
		SDL_Event e;
		while (SDL_PollEvent(&e)) {
			switch (e.type) {
				case SDL_QUIT:
					done=1;
				break;	
				
				case SDL_VIDEORESIZE: {
					int w=e.resize.w;
					int h=e.resize.h;
					
					if (w==scaled_width && h==scaled_width) break;
					
					if (w<32) w=32;
					if (h<32) h=32;
					if (w>original->w) w=original->w;
					if (h>original->h) h=original->h;
					
					if (w>scaled_width || h>scaled_height) {
						// we need to start over, won't do upscaling
						SDL_BlitSurface(original,NULL,scaled,NULL);
						scaled_width=original->w;
						scaled_height=original->h;
						shrink_surface(scaled,scaled_width-w,scaled_width,1);	
						shrink_surface(scaled,scaled_height-h,scaled_height,0);
						scaled_width=w;
						scaled_height=h;	
					} else {
						if (w<scaled_width) {
							shrink_surface(scaled,scaled_width-w,scaled_width,1);
							scaled_width=w;
						} 
						if (h<scaled_height) {
							shrink_surface(scaled,scaled_height-h,scaled_height,0);
							scaled_height=h;
						} 
					}
					
					
					
					screen=SDL_SetVideoMode(w,h,32,SDL_HWSURFACE|SDL_DOUBLEBUF|SDL_RESIZABLE);
				} break;
			}	
			
		}
		
		SDL_BlitSurface(scaled,NULL,screen,NULL);
		
		SDL_Flip(screen);
		SDL_Delay(10);
	}
	
	if (MessageBox(0,"Save changes?","Save",MB_YESNO)==IDYES) {
		SDL_SaveBMP(scaled,argv[1]);
	}
	
	SDL_FreeSurface(scaled);
	SDL_FreeSurface(original);
	
	return 0;
}
