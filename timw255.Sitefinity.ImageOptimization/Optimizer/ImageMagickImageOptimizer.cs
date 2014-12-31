﻿using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Libraries.Model;
using timw255.Sitefinity.ImageOptimization.Configuration;

namespace timw255.Sitefinity.ImageOptimization.Optimizer
{
    public class ImageMagickImageOptimizer : IImageOptimizer
    {
        private ImageOptimizationConfig _config;

        public Guid AlbumId { get; set; }

        public ImageMagickImageOptimizer()
        {
            _config = Config.Get<ImageOptimizationConfig>();
        }

        public Stream OptimizeImage(Image image, Stream imageData, out string optimizedFilename)
        {
            var settings = _config.Optimizers["ImageMagickImageOptimizer"].Parameters;

            int _imageQuality = Int32.Parse(settings["imageQuality"]);

            using (MemoryStream compressed = new MemoryStream())
            {
                MagickReadSettings magickSettings = new MagickReadSettings();

                switch (image.Extension)
                {
                    case ".png":
                        magickSettings.Format = MagickFormat.Png;
                        break;
                    case ".jpg":
                        magickSettings.Format = MagickFormat.Jpg;
                        break;
                    case ".jpeg":
                        magickSettings.Format = MagickFormat.Jpeg;
                        break;
                    case ".bmp":
                        magickSettings.Format = MagickFormat.Bmp;
                        break;
                    default:
                        magickSettings.Format = MagickFormat.Jpg;
                        break;
                }

                using (MagickImage img = new MagickImage(imageData, magickSettings))
                {
                    img.Quality = _imageQuality;
                    img.Write(compressed);

                    if (compressed == null)
                    {
                        optimizedFilename = "";
                        return null;
                    }

                    optimizedFilename = image.FilePath;
                    return compressed;
                }
            }
        }
    }
}