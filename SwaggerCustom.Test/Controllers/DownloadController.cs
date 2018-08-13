using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace SwaggerCustom.Test.Controllers
{
    /// <summary>
    /// 下载
    /// </summary>
    public class DownloadController : ApiController
    {

        private const int BufferSize = 80 * 1024;
        private const string MimeType = "application/octet-stream";
        public IFileProvider FileProvider { get; set; }
        public DownloadController()
        {
            FileProvider = new FileProvider();
        }

        // 下载文件路径
        private const string filePath = "G:/杂项.zip";
        private const string fileName = filePath;

        /// <summary>
        /// 普通下载
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult Get()
        {
            var browser = String.Empty;
            if (HttpContext.Current.Request.UserAgent != null)
            {
                browser = HttpContext.Current.Request.UserAgent.ToUpper();
            }
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            FileStream fileStream = File.OpenRead(filePath);
            httpResponseMessage.Content = new StreamContent(fileStream);
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeType);
            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = browser.Contains("FIREFOX")
                        ? Path.GetFileName(filePath)
                        : HttpUtility.UrlEncode(Path.GetFileName(filePath))
            };

            return ResponseMessage(httpResponseMessage);
        }

        /// <summary>
        /// 断点续传
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public IHttpActionResult Get(int i)
        {
            if (!FileProvider.Exists(fileName))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            long fileLength = FileProvider.GetLength(fileName);

            if (!FileProvider.Exists(fileName))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            fileLength = FileProvider.GetLength(fileName);
            var fileInfo = GetFileInfoFromRequest(this.Request, fileLength);

            var stream = new PartialContentFileStream(FileProvider.Open(fileName),
                                                 fileInfo.From, fileInfo.To);
            var response = new HttpResponseMessage();
            response.Content = new StreamContent(stream, BufferSize);
            SetResponseHeaders(response, fileInfo, fileLength, fileName);
            return ResponseMessage(response);
        }

        private FileInfo GetFileInfoFromRequest(HttpRequestMessage request, long entityLength)
        {
            var fileInfo = new FileInfo
            {
                From = 0,
                To = entityLength - 1,
                IsPartial = false,
                Length = entityLength
            };
            var rangeHeader = request.Headers.Range;
            if (rangeHeader != null && rangeHeader.Ranges.Count != 0)
            {
                if (rangeHeader.Ranges.Count > 1)
                {
                    throw new HttpResponseException(HttpStatusCode.RequestedRangeNotSatisfiable);
                }
                RangeItemHeaderValue range = rangeHeader.Ranges.First();
                if (range.From.HasValue && range.From < 0 || range.To.HasValue && range.To > entityLength - 1)
                {
                    throw new HttpResponseException(HttpStatusCode.RequestedRangeNotSatisfiable);
                }

                fileInfo.From = range.From ?? 0;
                fileInfo.To = range.To ?? entityLength - 1;
                fileInfo.IsPartial = true;
                fileInfo.Length = entityLength;
                if (range.From.HasValue && range.To.HasValue)
                {
                    fileInfo.Length = range.To.Value - range.From.Value + 1;
                }
                else if (range.From.HasValue)
                {
                    fileInfo.Length = entityLength - range.From.Value + 1;
                }
                else if (range.To.HasValue)
                {
                    fileInfo.Length = range.To.Value + 1;
                }
            }

            return fileInfo;
        }

        private void SetResponseHeaders(HttpResponseMessage response, FileInfo fileInfo,
                                      long fileLength, string fileName)
        {
            response.Headers.AcceptRanges.Add("bytes");
            response.StatusCode = fileInfo.IsPartial ? HttpStatusCode.PartialContent
                                      : HttpStatusCode.OK;
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = fileName;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeType);
            response.Content.Headers.ContentLength = fileInfo.Length;
            if (fileInfo.IsPartial)
            {
                response.Content.Headers.ContentRange
                    = new ContentRangeHeaderValue(fileInfo.From, fileInfo.To, fileLength);
            }
        }

        private HttpResponseMessage GetPartialContent(string fullFilePath, long partial)
        {
            var response = new HttpResponseMessage();
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(fullFilePath);
            long startByte = partial;
            var memoryStream = new MemoryStream();
            var buffer = new byte[65536];
            using (var fileStream = File.Open(fullFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var bytesRead = 0;
                fileStream.Seek(startByte, SeekOrigin.Begin);
                int length = Convert.ToInt32((fileInfo.Length - 1) - startByte) + 1;

                while (length > 0 && bytesRead > 0)
                {
                    bytesRead = fileStream.Read(buffer, 0, Math.Min(length, buffer.Length));
                    memoryStream.Write(buffer, 0, bytesRead);
                    length -= bytesRead;
                }
                response.Content = new StreamContent(memoryStream);
            }
            response.Headers.AcceptRanges.Add("bytes");
            response.StatusCode = HttpStatusCode.PartialContent;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeType);
            response.Content.Headers.ContentLength = File.Open(fullFilePath, FileMode.Open, FileAccess.Read, FileShare.Read).Length;
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = Path.GetFileName(fullFilePath)
            };
            return response;
        }
    }

    public class PartialContentFileStream : Stream
    {
        private readonly long _start;
        private readonly long _end;
        private long _position;
        private FileStream _fileStream;

        public override bool CanRead
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool CanSeek
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool CanWrite
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override long Length
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public PartialContentFileStream(FileStream fileStream, long start, long end)
        {
            _start = start;
            _position = start;
            _end = end;
            _fileStream = fileStream;

            if (start > 0)
            {
                _fileStream.Seek(start, SeekOrigin.Begin);
            }
        }

        /// <summary>
        /// 将缓冲区数据写到文件
        /// </summary>
        public override void Flush()
        {
            _fileStream.Flush();
        }

        /// <summary>
        /// 设置当前下载位置
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            if (origin == SeekOrigin.Begin)
            {
                _position = _start + offset;
                return _fileStream.Seek(_start + offset, origin);
            }
            else if (origin == SeekOrigin.Current)
            {
                _position += offset;
                return _fileStream.Seek(_position + offset, origin);
            }
            else
            {
                throw new NotImplementedException("SeekOrigin.End未实现");
            }
        }

        /// <summary>
        /// 依据偏离位置读取
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            int byteCountToRead = count;
            if (_position + count > _end)
            {
                byteCountToRead = (int)(_end - _position) + 1;
            }
            var result = _fileStream.Read(buffer, offset, byteCountToRead);
            _position += byteCountToRead;
            return result;
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            int byteCountToRead = count;
            if (_position + count > _end)
            {
                byteCountToRead = (int)(_end - _position);
            }
            var result = _fileStream.BeginRead(buffer, offset,
                                               count, (s) =>
                                               {
                                                   _position += byteCountToRead;
                                                   callback(s);
                                               }, state);
            return result;
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }

    public interface IFileProvider
    {
        bool Exists(string name);
        FileStream Open(string name);
        long GetLength(string name);
    }

    public class FileProvider : IFileProvider
    {
        private readonly string _filesDirectory;
        private const string AppSettingsKey = "DownloadDir";

        public FileProvider()
        {
            var fileLocation = ConfigurationManager.AppSettings[AppSettingsKey];
            if (!String.IsNullOrWhiteSpace(fileLocation))
            {
                _filesDirectory = fileLocation;
            }
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Exists(string name)
        {
            string file = Directory.GetFiles(_filesDirectory, name, SearchOption.TopDirectoryOnly)
                    .FirstOrDefault();
            return true;
        }


        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public FileStream Open(string name)
        {
            var fullFilePath = Path.Combine(_filesDirectory, name);
            return File.Open(fullFilePath,
                FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        /// <summary>
        /// 获取文件长度
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public long GetLength(string name)
        {
            var fullFilePath = Path.Combine(_filesDirectory, name);
            return new System.IO.FileInfo(fullFilePath).Length;
        }
    }

    public class FileInfo
    {
        public long From;
        public long To;
        public bool IsPartial;
        public long Length;
    }
}
