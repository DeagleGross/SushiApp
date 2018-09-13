using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using SelfHostApiServer.GlobalSettings;

namespace SelfHostApiServer.Models
{
    public class PhotoConverter
    {
        private string productType;
        private byte[] picNotFound; 

        public byte[] PicNotFound { get; }

        /// <summary>
        /// constructor by default
        /// </summary>
        public PhotoConverter(){}

        /// <summary>
        /// initializes an instance of converter for specific type of products
        /// </summary>
        /// <param name="productType">string containing productType</param>
        public PhotoConverter(string productType)
        {
            this.productType = productType;
            this.picNotFound = this.ConvertNotFound();
        }

        /// <summary>
        /// Converts the image to bytes by path in project
        /// </summary>
        /// <returns>The image to bytes.</returns>
        /// <param name="productName">Product name.</param>
        public byte[] ConvertImageToBytes(string productName)
        {
            byte[] array;

            try
            {
                string fullPath = Path.GetFullPath($@"../../PhotoDatabase/{this.productType}/{productName}.png");

                Image image = Image.FromFile(fullPath);

                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, ImageFormat.Png);
                    array = ms.ToArray();
                }

                return array;    
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Converts the not found picture for situations when no photo was downloaded to db
        /// </summary>
        /// <returns>The not found.</returns>
        public byte[] ConvertNotFound()
        {
            byte[] array;
            string fullPath = Path.GetFullPath($@"../../PhotoDatabase/image-not-found.jpg");

            Image image = Image.FromFile(fullPath);

            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                array = ms.ToArray();
            }

            return array;
        }
    }
}
 