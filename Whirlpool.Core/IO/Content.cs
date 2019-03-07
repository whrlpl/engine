using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whirlpool.Core.Pattern;
using Whirlpool.Core.Render;
using Whirlpool.Shared;
using RSG.Promises;
using RSG;

namespace Whirlpool.Core.IO
{

    public interface IAsset
    {
        string fileName { get; set; }
        byte[] data { get; set; }
        void LoadDataAsset(string fileName, byte[] data);
        void LoadAsset(PackageFile file);
    }

    public class MiscAsset : IAsset
    {
        string fileName;
        string IAsset.fileName { get { return fileName; } set { fileName = value; } }
        byte[] data;
        byte[] IAsset.data { get => data; set => data = value; }

        public string GetStringFromData()
        {
            return Encoding.ASCII.GetString(data);//.Remove(0, 0);
        }

        public void LoadAsset(PackageFile file)
        {
            fileName = file.fileName;
            data = file.fileData;
        }


        public void LoadDataAsset(string fileName, byte[] data)
        {
            this.fileName = fileName;
            this.data = data;
        }
    }


    public class Content : Singleton<Content>
    {
        public List<IAsset> assets = new List<IAsset>();


        private System.Type GetAssetType(string fileName)
        {
            string fileExt = fileName.Substring(fileName.LastIndexOf('.'));

            switch (fileExt)
            {
                case ".png":
                case ".jpg":
                case ".tga":
                case ".bmp":
                    return typeof(Texture);
                case ".png3d":
                case ".jpg3d":
                case ".bmp3d":
                    return typeof(Texture3D);
                case ".obj":
                    return typeof(Mesh);
                default:
                    return typeof(MiscAsset);
            }
        }

        private IAsset m_GetAsset(string fileName)
        {
            if (fileName.StartsWith("http"))
            {
                // Load from the web
                WebClient wc = new WebClient();
                IAsset asset = (IAsset)Activator.CreateInstance(GetAssetType(fileName));
                asset.LoadDataAsset(fileName, wc.DownloadData(fileName));
                Logging.Write("Loaded web asset '" + fileName + "'");
                wc.Dispose();
                return asset;
            }
            else
            {
                return LoadPackageAsset("\\" + fileName);
                //foreach (IAsset asset in assets)
                //{
                //    if (asset.fileName == "\\" + fileName)
                //    {
                //        return asset;
                //    }
                //}
            }
            throw new Exception("Asset '" + fileName + "' was not found.");

        }

        private Promise<IAsset> m_GetAssetAsync(string fileName)
        {
            var promise = new Promise<IAsset>();
            if (fileName.StartsWith("http"))
            {
                // Load from the web
                WebClient wc = new WebClient();
                IAsset asset = (IAsset)Activator.CreateInstance(GetAssetType(fileName));
                asset.LoadDataAsset(fileName, wc.DownloadData(fileName));
                Logging.Write("Loaded web asset '" + fileName + "'");
                wc.Dispose();
                promise.Resolve(asset);
            }
            else
            {
                LoadPackageAssetAsync("\\" + fileName).Then((IAsset asset) =>
                {
                    promise.Resolve(asset);
                });
            }
            return promise;

        }
        
        private static IAsset GetAsset(string fileName) => GetInstance().m_GetAsset(fileName);
        public static MiscAsset GetMiscAsset(string fileName) => (MiscAsset)GetAsset(fileName);
        public static Mesh GetMesh(string fileName) => (Mesh)GetAsset(fileName);
        public static Texture GetTexture(string fileName) => (Texture)GetAsset(fileName);
        public static Texture3D GetTexture3D(string fileName) => (Texture3D)GetAsset(fileName);

        public static Promise<IAsset> GetAssetAsync(string fileName) => GetInstance().m_GetAssetAsync(fileName);
        // TODO: simplify
        public static Promise<MiscAsset> GetMiscAssetAsync(string fileName)
        {
            var promise = new Promise<MiscAsset>();
            var asset = (MiscAsset)GetAsset(fileName);
            promise.Resolve(asset);
            return promise;
        }
        public static Promise<Mesh> GetMeshAsync(string fileName)
        {
            var promise = new Promise<Mesh>();
            var asset = (Mesh)GetAsset(fileName);
            promise.Resolve(asset);
            return promise;
        }
        public static Promise<Texture> GetTextureAsync(string fileName)
        {
            var promise = new Promise<Texture>();
            var asset = (Texture)GetAsset(fileName);
            promise.Resolve(asset);
            return promise;
        }
        public static Promise<Texture3D> GetTexture3DAsync(string fileName)
        {
            var promise = new Promise<Texture3D>();
            var asset = (Texture3D)GetAsset(fileName);
            promise.Resolve(asset);
            return promise;
        }

        public IAsset LoadPackageAsset(string packageFile)
        {
            Package p = new Package("Content.wpak");
            foreach (PackageFile file in p.files)
            {
                object o = Activator.CreateInstance(GetAssetType(file.fileName));
                if (file.fileName == packageFile)
                {
                    ((IAsset)o).LoadAsset(file);
                    Logging.Write("Loaded asset '" + file.fileName + "'");
                    return (IAsset)o;
                }
            }
            throw new Exception("Asset '" + packageFile + "' was not found.");
        }

        public Promise<IAsset> LoadPackageAssetAsync(string packageFile)
        {
            var promise = new Promise<IAsset>();
            Package p = new Package("Content.wpak");
            bool fileFound = false;
            foreach (PackageFile file in p.files)
            {
                object o = Activator.CreateInstance(GetAssetType(file.fileName));
                if (file.fileName == packageFile)
                {
                    ((IAsset)o).LoadAsset(file);
                    Logging.Write("Loaded asset '" + file.fileName + "'");
                    promise.Resolve((IAsset)o);
                    fileFound = true;
                    break;
                }
            }
            if (!fileFound)
                promise.Reject(new Exception("Asset '" + packageFile + "' was not found."));
            return promise;
        }
        
        public void LoadPackage(string packageLoc)
        {
            //Package p = new Package(packageLoc);
            //foreach (PackageFile file in p.files)
            //{
            //    object o = Activator.CreateInstance(GetAssetType(file.fileName));
            //    ((IAsset)o).LoadAsset(file);
            //    assets.Add((IAsset)o);
            //    Logging.Write("Loaded asset '" + file.fileName + "'");
            //}
        }
    }
}
