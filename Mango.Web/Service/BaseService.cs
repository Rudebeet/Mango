﻿using System;
using System.Net;
using System.Text;
using Mango.Web.Models;
using Mango.Web.Service.IService;
using Newtonsoft.Json;
using static Mango.Web.Utility.SD;

namespace Mango.Web.Service
{
	public class BaseService : IBaseService
	{
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;       
        }


        public HttpMethod Method { get; private set; }

        public async Task<ResponseDto?> SendAsync(RequestDto requestDto)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("MangoAPI");
                HttpRequestMessage message = new();

                message.Headers.Add("Accept", "application/json");
                //token

                message.RequestUri = new Uri(requestDto.Url);

                if (requestDto.Data is not null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
                }

                HttpResponseMessage? apiReponse = new();

                switch (requestDto.ApiVerb)
                {
                    case ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                apiReponse = await client.SendAsync(message);

                switch (apiReponse.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new() { IsSuccess = false, Message = "Not Found" };
                    case HttpStatusCode.Forbidden:
                        return new() { IsSuccess = false, Message = "Access Denied" };
                    case HttpStatusCode.Unauthorized:
                        return new() { IsSuccess = false, Message = "Unauthorized" };
                    case HttpStatusCode.InternalServerError:
                        return new() { IsSuccess = false, Message = "Internal Server Error" };
                    default:
                        var apiContent = await apiReponse.Content.ReadAsStringAsync();
                        var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                        return apiResponseDto;
                }
            }
            catch (Exception ex)
            {
                var dto = new ResponseDto()
                {
                    IsSuccess = false,
                    Message = ex.Message.ToString()
                };
            return dto;
            }
            
        }
    }
}

