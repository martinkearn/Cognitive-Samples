using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonGroupSample.Models
{
    public class Face
    {
        public string personGroupId { get; set; }
        public string personId { get; set; }
        public string userData { get; set; }
        public string targetFace { get; set; }
        public byte[] faceImage { get; set; }
    }
}
