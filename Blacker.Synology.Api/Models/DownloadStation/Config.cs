using System.Runtime.Serialization;

namespace Blacker.Synology.Api.Models.DownloadStation
{
    [DataContract]
    public class Config : ResponseConfigBase
    {
        [DataMember(Name = "bt_max_download")]
        private int? _btMaxDownload;
        [DataMember(Name = "bt_max_upload")]
        private int? _btMaxUpload;
        [DataMember(Name = "emule_enabled")]
        private bool? _eMuleEnabled;
        [DataMember(Name = "emule_max_download")]
        private int? _eMuleMaxDownload;
        [DataMember(Name = "emule_max_upload")]
        private int? _eMuleMaxUpload;
        [DataMember(Name = "ftp_max_download")]
        private int? _ftpMaxDownload;
        [DataMember(Name = "http_max_download")]
        private int? _httpMaxDownload;
        [DataMember(Name = "nzb_max_download")]
        private int? _nzbMaxDownload;
        [DataMember(Name = "unzip_service_enabled")]
        private bool? _unzipServiceEnabled;

        public int BtMaxDownload
        {
            get { return _btMaxDownload ?? default(int); }
            set { _btMaxDownload = value; }
        }

        public int BtMaxUpload
        {
            get { return _btMaxUpload ?? default(int); }
            set { _btMaxUpload = value; }
        }

        public bool EMuleEnabled
        {
            get { return _eMuleEnabled ?? default(bool); }
            set { _eMuleEnabled = value; }
        }

        public int EMuleMaxDownload
        {
            get { return _eMuleMaxDownload ?? default(int); }
            set { _eMuleMaxDownload = value; }
        }

        public int EMuleMaxUpload
        {
            get { return _eMuleMaxUpload ?? default(int); }
            set { _eMuleMaxUpload = value; }
        }

        public int FtpMaxDownload
        {
            get { return _ftpMaxDownload ?? default(int); }
            set { _ftpMaxDownload = value; }
        }

        public int HttpMaxDownload
        {
            get { return _httpMaxDownload ?? default(int); }
            set { _httpMaxDownload = value; }
        }

        public int NzbMaxDownload
        {
            get { return _nzbMaxDownload ?? default(int); }
            set { _nzbMaxDownload = value; }
        }

        public bool UnzipServiceEnabled
        {
            get { return _unzipServiceEnabled ?? default(bool); }
            set { _unzipServiceEnabled = value; }
        }

        [DataMember(Name = "default_destination")]
        public string DefaultDestination { get; set; }

        [DataMember(Name = "emule_default_destination")]
        public string EMuleDefaultDestination { get; set; }
    }
}
