namespace YourRest.WebApi.Options
{
    public class AwsOptions
    {
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string ServiceURL { get; set; }
        public BucketNamesOptions BucketNames { get; set; }
    }
}