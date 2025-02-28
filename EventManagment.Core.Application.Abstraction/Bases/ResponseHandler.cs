using System.Net;

namespace EventManagment.Core.Application.Abstraction.Bases
{
    public class ResponseHandler
    {

        public Response<T> Deleted<T>(string Message = null)
        {


            return new Response<T>()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = Message == null ? "Deleted Successfully" : Message
            };
        }
        public Response<T> Success<T>(T entity, object Meta = null!)
        {
            return new Response<T>()
            {
                Data = entity,
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = "Operation Successfully",
                Meta = Meta
            };
        }
        public Response<T> Unauthorized<T>(string Message = null!)
        {

            return new Response<T>()
            {
                StatusCode = System.Net.HttpStatusCode.Unauthorized,
                Succeeded = true,
                Message = Message == null ? "You Are Not Authorized" : Message
            };


        }
        public Response<T> BadRequest<T>(string Message = null!)
        {
            return new Response<T>()
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Succeeded = false,
                Message = Message == null ? "Bad Request" : Message
            };
        }

        public Response<T> NotFound<T>(object key, string message = null!)
        {
            return new Response<T>()
            {
                StatusCode = HttpStatusCode.NotFound,
                Succeeded = false,
                Message = message == null ? $"Not Found with key: {key}" : $"{message} with key: {key}"
            };
        }

        public Response<T> Created<T>(T entity, object Meta = null!)
        {
            return new Response<T>()
            {
                Data = entity,
                StatusCode = System.Net.HttpStatusCode.Created,
                Succeeded = true,
                Message = "Creation Succssefully",
                Meta = Meta
            };
        }
    }
}
