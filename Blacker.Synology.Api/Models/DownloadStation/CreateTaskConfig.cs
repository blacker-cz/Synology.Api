using System.Runtime.Serialization;

namespace Blacker.Synology.Api.Models.DownloadStation
{
    [DataContract]
    public class CreateTaskConfig : ResponseConfigBase
    {
        [DataMember(Name = "username")]
        public string Username { get; set; }

        [DataMember(Name = "password")]
        public string Password { get; set; }

        [DataMember(Name = "unzip_password")]
        public string UnzipPassword { get; set; }

        [DataMember(Name = "destination")]
        public string Destination { get; set; }
    }
}
