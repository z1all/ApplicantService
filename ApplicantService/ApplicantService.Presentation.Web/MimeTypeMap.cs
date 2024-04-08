namespace ApplicantService.Presentation.Web
{
    public static class MimeTypeMap
    {
        public static readonly Dictionary<string, string> _mappingToContentType;
        public static readonly Dictionary<string, string> _mappingToDocumentType;
   
        static MimeTypeMap()
        {
            List<Tuple<string, string>> mappings = new() {
                Tuple.Create(".jpe", "image/jpeg"),
                Tuple.Create(".jpeg", "image/jpeg"),
                Tuple.Create(".jpg", "image/jpeg"),
                Tuple.Create(".pdf", "application/pdf"),
                Tuple.Create(".png", "image/png"),
                Tuple.Create(".doc", "application/msword"),
                Tuple.Create(".docm", "application/vnd.ms-word.document.macroEnabled.12"),
                Tuple.Create(".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"),
                Tuple.Create(".dot", "application/msword")
            };
            
            Dictionary<string, string> mappingToContentType = new();
            Dictionary<string, string> mappingToDocumentType = new();
            foreach(var mapping in mappings)
            {
                mappingToContentType.Add(mapping.Item1, mapping.Item2);
                mappingToDocumentType.TryAdd(mapping.Item2, mapping.Item1);
            }
            _mappingToContentType = mappingToContentType;
            _mappingToDocumentType = mappingToContentType;
        }

        public static bool ExistType(string documentType)
        {
            return _mappingToDocumentType.ContainsKey(documentType);
        }

        public static string GetExistTypeString()
        {
            List<string> keys = _mappingToDocumentType.Keys.ToList();
            return string.Join(", ", keys);
        }

        public static bool TryMapToContentType(this string documentType, out string? contentType)
        {
            return _mappingToContentType.TryGetValue(documentType, out contentType);
        }

        public static bool TryMapToDocumentType(this string contentType, out string? documentType)
        {
            return _mappingToDocumentType.TryGetValue(contentType, out documentType);
        }
    }
}
