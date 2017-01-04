using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceVerify.Models
{
    public class Face
    {
        public string faceId { get; set; }
        public FaceRectangle faceRectangle { get; set; }
    }
}
