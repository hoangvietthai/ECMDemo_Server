using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Model
{
    public class Response<T> : Response
    {
        public T Data { get; set; }

        public int TotalCount { get; set; }

        public int DataCount { get; set; }

        public Response(int status, string message = null, T data = default(T))
            : base(status, message)
        {
            Data = data;
            TotalCount = 0;
            DataCount = 0;
        }

        public Response(int status, string message = null, T data = default(T), int dataCount = 0, int totalCount = 0)
            : base(status, message)
        {
            Data = data;
            TotalCount = totalCount;
            DataCount = dataCount;
        }
    }

    public class Response
    {
        public int Status { get; set; }

        public string Message { get; set; }

        public Response(int status, string message = null)
        {
            Status = status;
            Message = message;
        }
    }

    public class Response1<T> : Response
    {
        public T Data { get; set; }

        public Response1(int status, string message = null, T data = default(T))
            : base(status, message)
        {
            Data = data;
        }

    }
}