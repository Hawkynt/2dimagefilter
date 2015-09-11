# 2D-Image and Texture Filter #
This project tries to get all available image filters together, known to upscale lowres computer and console graphics. The goal is to modify them all to allow them to be used on a wide range of low-res graphics. One of the steps to achieve that is converting the filter algorithms, normally written to make comparisons like
> `(color1==color2)?color1:color3`
into something like that
> `(color1 IsLike color2)?Interpolate(color1,color2):color3`

I'm trying not to use code from other projects directly, but I implement their algo's in a similar way.

As of now (2015) this project has become a reference for much more image resampling algorithms. Even very exotic windowing functions found their way into the code, so the next goal is more like getting each possible available rescaling algo into the library.

---

```
So credits go to the following scalers:
Eagle (the godfather himself)
Super Eagle (thanks Kreed and ZSNES)
SaI2x, Super2xSaI (also Kreed and DOSBox)
Scale2x, Scale3x (thanks MAME for these)
AdvInterp2x, AdvInterp3x (also MAME)
HQ2x, HQ3x, HQ4x (Maxim Stepin)
LQ2x, LQ3x, LQ4x (AFAIK SNES9x but AdvMAME also)
HQ2x3, HQ2x4, LQ2x3, LQ2x4 (AdvMAME again)
nQx Bold and Smart Version (SNES9x, VirtualBoyAdvance)
Bilinear Plus Original and Modified (VBA-rr)
XBR2x, XBR3x, XBR4x Normal and NonBlend (thanks Hyllian)
Resampling kernels (Pascal Getreuer)
XBRz (Zenju)
SCL, DES (FNES)
```



# Prerequisites #
  * .NET Framework 4.5


# Downloads #
  * current release v1.1.3.3 ([r133](https://code.google.com/p/2dimagefilter/source/detail?r=133))
  * are on the left side in the "External Links" section