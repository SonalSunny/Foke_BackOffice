namespace FOKE.Entity.FileUpload
{
    public static class FileUploadCategories
    {
        public const long ScheduleTask = 1;
        public const long GeneralTask = 2;
        public const long SupportTask = 3;
        public const long RequirementTask = 4;
        public const long CircularFileupload = 5;

        public static string GetCategoryName(long? value)
        {
            switch (value)
            {
                case ScheduleTask:
                    return nameof(ScheduleTask);
                case GeneralTask:
                    return nameof(GeneralTask);
                case SupportTask:
                    return nameof(SupportTask);
                case RequirementTask:
                    return nameof(RequirementTask);
                case CircularFileupload:
                    return nameof(CircularFileupload);
                default:
                    return "General";
            }
        }
    }
}
