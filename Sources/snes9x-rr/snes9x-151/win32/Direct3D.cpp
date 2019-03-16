/* Direct3D.cpp - written by OV2 */

#pragma comment( lib, "d3d9" )
#pragma comment( lib, "d3dx9" )
#pragma comment( lib, "DxErr9" )

#include "../snes9x.h"

#include <Dxerr.h>
#include <tchar.h>
#include "direct3d.h"
#include "../gfx.h"
#include "wsnes9x.h"
#include <commctrl.h>

extern bool in_display_dlg;
void ConvertDepth (SSurface *src, SSurface *dst, RECT *);
int Init_2xSaI (uint32 BitFormat);
#define RenderMethod ((Src.Height > SNES_HEIGHT_EXTENDED || Src.Width == 512) ? RenderMethodHiRes : RenderMethod)

/* CDirect3D::CDirect3D()
sets default values for the variables
*/
CDirect3D::CDirect3D()
{
	init_done = false;
	pD3D = NULL;
	pDevice = NULL;
	drawSurface = NULL;
	vertexBuffer = NULL;
	afterRenderWidth = 0;
	afterRenderHeight = 0;
	quadTextureSize = 0;
	fullscreen = false;
	iFilterScale = 0;
}

/* CDirect3D::~CDirect3D()
releases allocated objects
*/
CDirect3D::~CDirect3D()
{
	deInitialize();
}


/*  CDirect3D::initialize
initializes Direct3D (always in window mode)
IN:
hWnd	-	the HWND of the window in which we render/the focus window for fullscreen
-----
returns true if successful, false otherwise
*/
bool CDirect3D::initialize(HWND hWnd)
{
	if(init_done)
		return true;

	pD3D = Direct3DCreate9(D3D_SDK_VERSION);
	if(pD3D == NULL) {
		DXTRACE_ERR_MSGBOX(TEXT("Error creating initial D3D9 object"), 0);
		return false;
	}

	ZeroMemory(&dPresentParams, sizeof(dPresentParams));
	dPresentParams.hDeviceWindow = hWnd;
    dPresentParams.Windowed = TRUE;
	dPresentParams.BackBufferCount = GUI.tripleBuffering?2:1;
    dPresentParams.SwapEffect = D3DSWAPEFFECT_DISCARD;
	dPresentParams.BackBufferFormat = D3DFMT_UNKNOWN;

	HRESULT hr = pD3D->CreateDevice(D3DADAPTER_DEFAULT,
                      D3DDEVTYPE_HAL,
                      hWnd,
                      D3DCREATE_MIXED_VERTEXPROCESSING
                        | D3DCREATE_FPU_PRESERVE, // mainly for lua modules
                      &dPresentParams,
                      &pDevice);
	if(FAILED(hr)) {
		DXTRACE_ERR_MSGBOX(TEXT("Error creating D3D9 device"), hr);
		return false;
	}

	hr = pDevice->CreateVertexBuffer(sizeof(triangleStripVertices),D3DUSAGE_WRITEONLY,FVF_COORDS_TEX,D3DPOOL_MANAGED,&vertexBuffer,NULL);
	if(FAILED(hr)) {
		DXTRACE_ERR_MSGBOX(TEXT("Error creating vertex buffer"), hr);
		return false;
	}

	if(GUI.d3dFilter == BILINEAR) {
		pDevice->SetSamplerState(0, D3DSAMP_MAGFILTER, D3DTEXF_LINEAR);
		pDevice->SetSamplerState(0, D3DSAMP_MINFILTER, D3DTEXF_LINEAR);
	} else {
		pDevice->SetSamplerState(0, D3DSAMP_MAGFILTER, D3DTEXF_POINT);
		pDevice->SetSamplerState(0, D3DSAMP_MINFILTER, D3DTEXF_POINT);
	}

	pDevice->Clear(0, NULL, D3DCLEAR_TARGET, D3DCOLOR_XRGB(0, 0, 0), 1.0f, 0);

	init_done = true;

	return true;

}

void CDirect3D::deInitialize()
{
	destroyDrawSurface();

	if(vertexBuffer) {
		vertexBuffer->Release();
		vertexBuffer = NULL;
	}

	if( pDevice ) {
		pDevice->Release();
		pDevice = NULL;
	}

	if( pD3D ) {
		pD3D->Release();
		pD3D = NULL;
	}

	init_done = false;
	afterRenderWidth = 0;
	afterRenderHeight = 0;
	quadTextureSize = 0;
	fullscreen = false;
	iFilterScale = 0;
}

/*  CDirect3D::render
does the actual rendering, changes the draw surface if necessary and recalculates
the vertex information if filter output size changes
IN:
Src		-	the input surface
*/
void CDirect3D::render(SSurface Src)
{
	SSurface Dst;
	RECT dstRect;
	int iNewFilterScale;
	D3DLOCKED_RECT lr;
	//D3DLOCKED_RECT lrConv;
	HRESULT hr;

	if(!init_done) return;

	//create a new draw surface if the filter scale changes
	//at least factor 2 so we can display unscaled hi-res images
	iNewFilterScale = max(2,max(GetFilterScale(GUI.ScaleHiRes),GetFilterScale(GUI.Scale)));
	if(iNewFilterScale!=iFilterScale) {
		changeDrawSurfaceSize(iNewFilterScale);
	}

	if(FAILED(hr = pDevice->TestCooperativeLevel())) {
		switch(hr) {
			case D3DERR_DEVICELOST:		//do no rendering until device is restored
				return;
			case D3DERR_DEVICENOTRESET: //we can reset now
				resetDevice();
				return;
			default:
				DXTRACE_ERR( TEXT("Internal driver error"), hr);
				return;
		}
	}

	//blankTexture(drawSurface);
	if(FAILED(hr = drawSurface->LockRect(0, &lr, NULL, 0))) {
		DXTRACE_ERR_MSGBOX( TEXT("Unable to lock texture"), hr);
		return;
	} else {
		Dst.Surface = (unsigned char *)lr.pBits;
		Dst.Height = quadTextureSize;
		Dst.Width = quadTextureSize;
		Dst.Pitch = lr.Pitch;

		RenderMethod (Src, Dst, &dstRect);
		if(!Settings.AutoDisplayMessages)
			S9xDisplayMessages ((uint16*)Dst.Surface, Dst.Pitch/2, dstRect.right-dstRect.left, dstRect.bottom-dstRect.top - ((in_display_dlg && GUI.HeightExtend) ? GetFilterScale(GUI.Scale) : 0), GetFilterScale(GUI.Scale));

		drawSurface->UnlockRect(0);
	}

	//if the output size of the render method changes we need new vertices
	if(afterRenderHeight != dstRect.bottom || afterRenderWidth != dstRect.right) {
		afterRenderHeight = dstRect.bottom;
		afterRenderWidth = dstRect.right;
		setupVertices();
	}

	if(!GUI.Stretch||GUI.AspectRatio)
		pDevice->Clear(0, NULL, D3DCLEAR_TARGET, D3DCOLOR_XRGB(0, 0, 0), 1.0f, 0);
	
	pDevice->BeginScene();

	pDevice->SetTexture(0, drawSurface);
	pDevice->SetFVF(FVF_COORDS_TEX);
	pDevice->SetStreamSource(0,vertexBuffer,0,sizeof(VERTEX));
	pDevice->DrawPrimitive(D3DPT_TRIANGLESTRIP,0,2);

    pDevice->EndScene();

	pDevice->Present(NULL, NULL, NULL, NULL);

    return;
}

/*  CDirect3D::createDrawSurface
calculates the necessary texture size (multiples of 2)
and creates a new texture
*/
void CDirect3D::createDrawSurface()
{
	int neededSize;
	HRESULT hr;

	//we need at least 512 pixels (SNES_WIDTH * 2) so we can start with that value
	quadTextureSize = 512;
	neededSize = SNES_WIDTH * iFilterScale;
	while((int)quadTextureSize < neededSize)
		quadTextureSize *=2;

	if(!drawSurface) {
		hr = pDevice->CreateTexture(
			quadTextureSize, quadTextureSize,
			1, // 1 level, no mipmaps
			0, // dynamic textures can be locked
			D3DFMT_R5G6B5,
			D3DPOOL_MANAGED,
			&drawSurface,
			NULL );

		if(FAILED(hr)) {
			DXTRACE_ERR_MSGBOX(TEXT("Error while creating texture"), hr);
			return;
		}
	}
}

/*  CDirect3D::destroyDrawSurface
releases the old textures (if allocated)
*/
void CDirect3D::destroyDrawSurface()
{
	if(drawSurface) {
		drawSurface->Release();
		drawSurface = NULL;
	}
}

/*  CDirect3D::blankTexture
clears a texture (fills it with zeroes)
IN:
texture		-	the texture to be blanked
-----
returns true if successful, false otherwise
*/
bool CDirect3D::blankTexture(LPDIRECT3DTEXTURE9 texture)
{
	D3DLOCKED_RECT lr;
	HRESULT hr;

	if(FAILED(hr = texture->LockRect(0, &lr, NULL, 0))) {
		DXTRACE_ERR_MSGBOX( TEXT("Unable to lock texture"), hr);
		return false;
	} else {
		memset(lr.pBits, 0, lr.Pitch * quadTextureSize);
		texture->UnlockRect(0);
		return true;
	}
}

/*  CDirect3D::changeDrawSurfaceSize
changes the draw surface size: deletes the old textures, creates a new texture
and calculate new vertices
IN:
iScale	-	the scale that has to fit into the textures
-----
returns true if successful, false otherwise
*/
bool CDirect3D::changeDrawSurfaceSize(int iScale)
{
	iFilterScale = iScale;

	if(pDevice) {
		destroyDrawSurface();
		createDrawSurface();
		setupVertices();
		return true;
	}
	return false;
}

/*  CDirect3D::setupVertices
calculates the vertex coordinates
(respecting the stretch and aspect ratio settings)
*/
void CDirect3D::setupVertices()
{
	float xFactor;
	float yFactor;
	float minFactor;
	float renderWidthCalc,renderHeightCalc;
	RECT drawRect;
	int hExtend = GUI.HeightExtend ? SNES_HEIGHT_EXTENDED : SNES_HEIGHT;
	float snesAspect = (float)GUI.AspectWidth/hExtend;
	void *pLockedVertexBuffer;

	if(GUI.Stretch) {
		if(GUI.AspectRatio) {
			//fix for hi-res images with FILTER_NONE
			//where we need to correct the aspect ratio
			renderWidthCalc = (float)afterRenderWidth;
			renderHeightCalc = (float)afterRenderHeight;
			if(renderWidthCalc/renderHeightCalc>snesAspect)
				renderWidthCalc = renderHeightCalc * snesAspect;
			else if(renderWidthCalc/renderHeightCalc<snesAspect)
				renderHeightCalc = renderWidthCalc / snesAspect;

			xFactor = (float)dPresentParams.BackBufferWidth / renderWidthCalc;
			yFactor = (float)dPresentParams.BackBufferHeight / renderHeightCalc;
			minFactor = xFactor < yFactor ? xFactor : yFactor;

			drawRect.right = (LONG)(renderWidthCalc * minFactor);
			drawRect.bottom = (LONG)(renderHeightCalc * minFactor);

			drawRect.left = (dPresentParams.BackBufferWidth - drawRect.right) / 2;
			drawRect.top = (dPresentParams.BackBufferHeight - drawRect.bottom) / 2;
			drawRect.right += drawRect.left;
			drawRect.bottom += drawRect.top;
		} else {
			drawRect.top = 0;
			drawRect.left = 0;
			drawRect.right = dPresentParams.BackBufferWidth;
			drawRect.bottom = dPresentParams.BackBufferHeight;
		}
	} else {
		drawRect.left = ((int)(dPresentParams.BackBufferWidth) - (int)afterRenderWidth) / 2;
		drawRect.top = ((int)(dPresentParams.BackBufferHeight) - (int)afterRenderHeight) / 2;
		if(drawRect.left < 0) drawRect.left = 0;
		if(drawRect.top < 0) drawRect.top = 0;
		drawRect.right = drawRect.left + afterRenderWidth;
		drawRect.bottom = drawRect.top + afterRenderHeight;

		//the lines below would downsize the image if window size is smaller than the output, but
		//since the old directdraw code did not do that I've left it out
		//if(drawRect.right > dPresentParams.BackBufferWidth) drawRect.right = dPresentParams.BackBufferWidth;
		//if(drawRect.bottom > dPresentParams.BackBufferHeight) drawRect.bottom = dPresentParams.BackBufferHeight;
	}

	float tX = (float)afterRenderWidth / (float)quadTextureSize;
	float tY = (float)afterRenderHeight / (float)quadTextureSize;

	//we need to substract -0.5 from the x/y coordinates to match texture with pixel space
	//see http://msdn.microsoft.com/en-us/library/bb219690(VS.85).aspx
	triangleStripVertices[0] = VERTEX((float)drawRect.left - 0.5f,(float)drawRect.bottom - 0.5f,0.0f,1.0f,0.0f,tY);
	triangleStripVertices[1] = VERTEX((float)drawRect.left - 0.5f,(float)drawRect.top - 0.5f,0.0f,1.0f,0.0f,0.0f);
	triangleStripVertices[2] = VERTEX((float)drawRect.right - 0.5f,(float)drawRect.bottom - 0.5f,0.0f,1.0f,tX,tY);
	triangleStripVertices[3] = VERTEX((float)drawRect.right - 0.5f,(float)drawRect.top - 0.5f,0.0f,1.0f,tX,0.0f);

	HRESULT hr = vertexBuffer->Lock(0,0,&pLockedVertexBuffer,NULL);
	memcpy(pLockedVertexBuffer,triangleStripVertices,sizeof(triangleStripVertices));
	vertexBuffer->Unlock();
}

/*  CDirect3D::changeRenderSize
determines if we need to reset the device (if the size changed)
called with (0,0) whenever we want new settings to take effect
IN:
newWidth,newHeight	-	the new window size
-----
returns true if successful, false otherwise
*/
bool CDirect3D::changeRenderSize(unsigned int newWidth, unsigned int newHeight)
{
	if(!init_done)
		return false;

	//if we already have the wanted size no change is necessary
	//during fullscreen no changes are allowed
	if(fullscreen || dPresentParams.BackBufferWidth == newWidth && dPresentParams.BackBufferHeight == newHeight)
		return true;

	if(!resetDevice())
		return false;
	setupVertices();
	return true;
}

/*  CDirect3D::resetDevice
resets the device
called if surface was lost or the settings/display size require a device reset
-----
returns true if successful, false otherwise
*/
bool CDirect3D::resetDevice()
{
	if(!pDevice) return false;

	HRESULT hr;

	//release prior to reset
	destroyDrawSurface();

	//zero or unknown values result in the current window size/display settings
	dPresentParams.BackBufferWidth = 0;
	dPresentParams.BackBufferHeight = 0;
	dPresentParams.BackBufferCount = GUI.tripleBuffering?2:1;
	dPresentParams.BackBufferFormat = D3DFMT_UNKNOWN;
	dPresentParams.FullScreen_RefreshRateInHz = 0;
	dPresentParams.Windowed = true;
	dPresentParams.PresentationInterval = GUI.Vsync?D3DPRESENT_INTERVAL_ONE:D3DPRESENT_INTERVAL_IMMEDIATE;

	if(fullscreen) {
		dPresentParams.BackBufferWidth = GUI.Width;
		dPresentParams.BackBufferHeight = GUI.Height;
		dPresentParams.BackBufferCount = GUI.tripleBuffering?2:1;
		dPresentParams.Windowed = false;
		if(GUI.Depth == 32)
			dPresentParams.BackBufferFormat = D3DFMT_X8R8G8B8;
		else
			dPresentParams.BackBufferFormat = D3DFMT_R5G6B5;
		dPresentParams.FullScreen_RefreshRateInHz = GUI.RefreshRate;
	}

	if(FAILED(hr = pDevice->Reset(&dPresentParams))) {
		DXTRACE_ERR(TEXT("Unable to reset device"), hr);
		return false;
	}

	if(GUI.d3dFilter == BILINEAR) {
		pDevice->SetSamplerState(0, D3DSAMP_MAGFILTER, D3DTEXF_LINEAR);
		pDevice->SetSamplerState(0, D3DSAMP_MINFILTER, D3DTEXF_LINEAR);
	} else {
		pDevice->SetSamplerState(0, D3DSAMP_MAGFILTER, D3DTEXF_POINT);
		pDevice->SetSamplerState(0, D3DSAMP_MINFILTER, D3DTEXF_POINT);
	}

	pDevice->Clear(0, NULL, D3DCLEAR_TARGET, D3DCOLOR_XRGB(0, 0, 0), 1.0f, 0);
	
	//recreate the surface
	createDrawSurface();
	return true;
}

/*  CDirect3D::setSnes9xColorFormat
sets the color format to 16bit (since the texture is always 16bit)
no depth conversion is necessary (done by D3D)
*/
void CDirect3D::setSnes9xColorFormat()
{
	GUI.ScreenDepth = 16;
	GUI.BlueShift = 0;
	GUI.GreenShift = 6;
	GUI.RedShift = 11;
	S9xSetRenderPixelFormat (RGB565);
	Init_2xSaI (565);
	GUI.NeedDepthConvert = FALSE;
	GUI.DepthConverted = TRUE;
	return;
}

/*  CDirect3D::setFullscreen
enables/disables fullscreen mode
IN:
fullscreen	-	determines if fullscreen is enabled/disabled
-----
returns true if successful, false otherwise
*/
bool CDirect3D::setFullscreen(bool fullscreen)
{
	if(!init_done)
		return false;

	if(this->fullscreen==fullscreen)
		return true;

	this->fullscreen = fullscreen;
	if(!resetDevice())
		return false;

	//present here to get a fullscreen blank even if no rendering is done
	pDevice->Present(NULL,NULL,NULL,NULL);
	setupVertices();
	return true;
}

/*  CDirect3D::fillModesListView
enumerates possible display modes (only 16 and 32 bit) and fills the list view
and mode vector with those values
IN:
listView	-	the HWND of the list view to fill
modeVector	-	pointer to the mode vector
*/
void CDirect3D::fillModesListView(HWND listView,std::vector<dMode> *modeVector)
{
	D3DDISPLAYMODE d3dMode;
	int modeCount,index;
	TCHAR depmode[80];
	dMode mode;
	LVITEM lvi;

	if(!init_done)
		return;

	//enumerate 32bit modes
	modeCount = pD3D->GetAdapterModeCount(D3DADAPTER_DEFAULT,D3DFMT_X8R8G8B8);
	for(int i=0;i<modeCount;i++) {
		if(pD3D->EnumAdapterModes(D3DADAPTER_DEFAULT,D3DFMT_X8R8G8B8,i,&d3dMode)==D3D_OK) {
			sprintf( depmode, "%dx%d 32bit %dhz", d3dMode.Width, d3dMode.Height,d3dMode.RefreshRate);
			mode.width = d3dMode.Width;
			mode.height = d3dMode.Height;
			mode.rate = d3dMode.RefreshRate;
			mode.depth = 32;
			mode.status = 3;

			modeVector->push_back(mode);
			ZeroMemory(&lvi, sizeof(LVITEM));
			lvi.iItem=modeVector->size();
			lvi.mask=LVIF_TEXT;
			lvi.pszText=depmode;
			lvi.cchTextMax=80;
			index=ListView_InsertItem(listView, &lvi);
		}

	}

	//enumerate 16bit modes
	modeCount = pD3D->GetAdapterModeCount(D3DADAPTER_DEFAULT,D3DFMT_R5G6B5);
	for(int i=0;i<modeCount;i++) {
		if(pD3D->EnumAdapterModes(D3DADAPTER_DEFAULT,D3DFMT_R5G6B5,i,&d3dMode)==D3D_OK) {
			sprintf( depmode, "%dx%d 16bit %dhz", d3dMode.Width, d3dMode.Height,d3dMode.RefreshRate);
			mode.width = d3dMode.Width;
			mode.height = d3dMode.Height;
			mode.rate = d3dMode.RefreshRate;
			mode.depth = 16;
			mode.status = 3;

			modeVector->push_back(mode);
			ZeroMemory(&lvi, sizeof(LVITEM));
			lvi.iItem=modeVector->size();
			lvi.mask=LVIF_TEXT;
			lvi.pszText=depmode;
			lvi.cchTextMax=80;
			index=ListView_InsertItem(listView, &lvi);
		}

	}
}