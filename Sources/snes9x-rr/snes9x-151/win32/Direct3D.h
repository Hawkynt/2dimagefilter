/* Direct3D.h - written by OV2 */
#ifndef W9XDIRECT3D_H
#define W9XDIRECT3D_H

#include <windows.h>
#include <d3d9.h>
#include "render.h"
#include <vector>
#include "wsnes9x.h"

#define FVF_COORDS_TEX D3DFVF_XYZRHW | D3DFVF_TEX1

typedef struct _VERTEX {
		float x, y, z;
		float rhw;
		float tx, ty;
		_VERTEX() {}
		_VERTEX(float x,float y,float z,float rhw,float tx,float ty) {
			this->x=x;this->y=y;this->z=z;this->rhw=rhw;this->tx=tx;this->ty=ty;
		}
} VERTEX; //our custom vertex with a constuctor for easier assignment

class CDirect3D
{
private:
	bool                  init_done;					//has initialize been called?
	LPDIRECT3D9           pD3D;
	LPDIRECT3DDEVICE9     pDevice;
	LPDIRECT3DTEXTURE9    drawSurface;					//the texture used for all drawing operations

	LPDIRECT3DVERTEXBUFFER9 vertexBuffer;
	D3DPRESENT_PARAMETERS dPresentParams;
	int iFilterScale;									//the current maximum filter scale (at least 2)
	unsigned int afterRenderWidth, afterRenderHeight;	//dimensions after filter has been applied
	unsigned int quadTextureSize;						//size of the texture (only multiples of 2)
	bool fullscreen;									//are we currently displaying in fullscreen mode
	
	VERTEX triangleStripVertices[4];					//the 4 vertices that make up our display rectangle

	bool blankTexture(LPDIRECT3DTEXTURE9 texture);
	void createDrawSurface();
	void destroyDrawSurface();
	bool changeDrawSurfaceSize(int iScale);
	void setupVertices();
	bool resetDevice();

public:
	CDirect3D();
	~CDirect3D();
	bool initialize(HWND hWnd);
	void deInitialize();
	void render(SSurface Src);
	bool changeRenderSize(unsigned int newWidth, unsigned int newHeight);
	bool setFullscreen(bool fullscreen);
	void setSnes9xColorFormat();
	void fillModesListView(HWND listView,std::vector<dMode> *modeVector);
};

#endif