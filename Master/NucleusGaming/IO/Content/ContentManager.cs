﻿using Nucleus.Gaming.Coop;
using Nucleus.Gaming.Package;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Nucleus.Gaming
{
    public class ContentManager : IDisposable
    {
        public static readonly string AssetsFolder = "assets";

        private Dictionary<string, Image> loadedImages;
        private bool isDisposed;
        private HandlerData game;
        private string handlersFolder;
        private string pkgFolder;

        public Image DefaultImage { get; set; }

        public ContentManager(GameHandlerMetadata info, HandlerData game)
        {
            this.game = game;
            loadedImages = new Dictionary<string, Image>();

            handlersFolder = GameManager.Instance.GetInstalledPackagePath();
            pkgFolder = PackageManager.GetInstallPath(info);

            DefaultImage = new Bitmap(1, 1);
        }

        public void Dispose()
        {
            if (isDisposed)
            {
                return;
            }

            isDisposed = true;
            foreach (Image image in loadedImages.Values)
            {
                image.Dispose();
            }
            loadedImages = null;
        }

        public Image LoadImage(string url)
        {
            // clear the url
            url = url.ToLower();
            Image img;
            if (loadedImages.TryGetValue(url, out img))
            {
                return img;
            }

            string fullPath = Path.Combine(pkgFolder, AssetsFolder, url);
            if (!File.Exists(fullPath))
            {
                return DefaultImage;
            }

            img = Image.FromFile(fullPath);
            loadedImages.Add(url, img);
            return img;
        }
    }
}
