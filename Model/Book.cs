using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBookFront.Model
{
    public class Book
    {
         
        public string BookName { get; set; }

        public byte[] ImageByte;
        public string Image
        {
            get => Convert.ToBase64String(ImageByte);
            set => ImageByte = Convert.FromBase64String(value);      
        }
}
}
