namespace StorageFabricBackend.Models
{
    public class DeploymentRequest {
        public string Region { get; set; }
        public int NodeCount { get; set; }
        public string StorageSize { get; set; }

        public string VMType { get; set; }
    }
}