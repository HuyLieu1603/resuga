using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PD_Store.Helper
{
    public class Contants
    {
        public const string NotFound = "Not Found";
        public const string Successed = "Successed";
        public const string BadRequest = "Bad Request";
        public const string InternalServerError = "Internal Server Error";
        public const int StatusCodeSuccessed = 200;
        public const int StatusCodeBadRequest = 400;
        public const int StatusCodeNotFound = 404;
        public const int StatusCodeInternalServerError = 500;

        public const string DefaultPassword = "Nmv@1231";

        public const string DefaultMessageError = "Hệ thống đã xảy ra lỗi! Vui lòng liện hệ IT Admin để giải quyết";
    }
}