
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Drawing;
//using System.Drawing.Imaging;

//namespace DLIDefinitions
//{
//    class KeyGenerator
//    {

//        public static Bitmap GenerateKey(Bitmap bitmap, bool invert)
//        {
//            return MakeTheKey(bitmap, true, 0, invert);
//        }

//        public static Bitmap GenerateKey(Bitmap bitmap, bool invert, int nonLinearCutoff)
//        {
//            return MakeTheKey(bitmap, false, nonLinearCutoff, invert);
//        }

//        public static Bitmap MakeTheKey(Bitmap bitmap, bool isLinear, int nonLinearCutoff, bool invert)
//        {
//            Bitmap kbmp = new Bitmap(bitmap);
//            BitmapData bmData = kbmp.LockBits(new Rectangle(0, 0, kbmp.Width, kbmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
//            int stride = bmData.Stride;
//            System.IntPtr Scan0 = bmData.Scan0;
//            byte alpha;
//            unsafe {
//                byte* p = (byte*)(void*)Scan0;
//                int nOffset = stride - kbmp.Width * 4;
//                int nWidth = kbmp.Width * 4;
//                for (int y = 0; y < kbmp.Height; ++y) {
//                    for (int x = 0; x < kbmp.Width; ++x) {
//                        alpha = p[3];
//                        if (isLinear && !invert) {
//                            p[0] = p[1] = p[2] = alpha;
//                            p[3] = 255;
//                        }
//                        else if (isLinear && invert) {
//                            p[0] = p[1] = p[2] = ((byte)(255 - alpha));
//                            p[3] = 255;
//                        }
//                        else if (!isLinear && !invert) {
//                            if (alpha < nonLinearCutoff) {
//                                p[0] = p[1] = p[2] = 0;
//                                p[3] = 255;
//                            }
//                            else {
//                                p[0] = p[1] = p[2] = p[3] = 255;
//                            }
//                        }
//                        else if (!isLinear && invert) {
//                            if (alpha < nonLinearCutoff) {
//                                p[0] = p[1] = p[2] = 255;
//                                p[3] = 255;
//                            }
//                            else {
//                                p[0] = p[1] = p[2] = p[3] = 0;
//                            }
//                        }
//                        p += 4;
//                    }
//                    p += nOffset;
//                }
//            }

//            kbmp.UnlockBits(bmData);
//            return kbmp;
//        }
//    }
//}
