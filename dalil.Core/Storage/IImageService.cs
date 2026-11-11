using System;
using System.Collections.Generic;
using System.Text;

namespace dalil.Core.Storage
{
    public interface IImageService
    {
        Task<string> SaveCourseImageAsync(Stream content, string fileName, long length);
        void DeleteCourseImage(string? imageUrl);
    }
}
