using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Mango.Services.CoupanAPI.Models;
using Mango.Services.CoupanAPI.Models.DTO;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    public class CouponAPIController : Controller
    {
        private readonly AppDbContext _db;
        private ResponseDto responseDto;
        private readonly IMapper _mapper;

        public CouponAPIController(AppDbContext dbContext, IMapper mapper)
        {
            _db = dbContext;
            _mapper = mapper;

            responseDto = new ResponseDto();
        }

        // GET: api/values
        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Coupon> coupons = _db.Coupons.ToList();
                responseDto.Result  = _mapper.Map< IEnumerable<CouponDto>>(coupons);
            }
            catch(Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = ex.Message;
            }
            return responseDto;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ResponseDto Get(int id)
        {
            try
            {
                Coupon? coupon = _db.Coupons.FirstOrDefault(u => u.CouponId == id);
                responseDto.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch(Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = ex.Message;
            }
            return responseDto;
        }

        // GET api/values/100OFF
        [HttpGet]
        [Route("GetByCode/{couponCode}")]
        public ResponseDto Get(String couponCode)
        {
            try
            {
                Coupon? coupon = _db.Coupons.FirstOrDefault(u => u.CouponCode.ToLower() == couponCode.ToLower());
                if(coupon is null)
                {
                    responseDto.IsSuccess = false;
                    responseDto.Message = "Invalid Coupon";
                }
                responseDto.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = ex.Message;
            }
            return responseDto;
        }

        // POST api/values
        [HttpPost]
        public ResponseDto Post([FromBody]CouponDto couponDto)
        {
            try
            {
                Coupon coupon = _mapper.Map<Coupon>(couponDto);
                _db.Coupons.Add(coupon);
                _db.SaveChanges();
                responseDto.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch(Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = ex.Message;
            }
            return responseDto;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public ResponseDto Put(int id,[FromBody]CouponDto coupondto)
        {
            
            try
            {
                //Check whether coupon is available
                Coupon? coupon = _db.Coupons.FirstOrDefault(u => u.CouponId == id);
                if (coupon is null)
                {
                    responseDto.IsSuccess = false;
                    responseDto.Message = "No Coupon Found";
                    return responseDto;
                }

                //Assigning new values for coupon object
                coupon.CouponCode = coupondto.CouponCode;
                coupon.DiscountAmount = coupondto.DiscountAmount;
                coupon.MinAmount = coupondto.MinAmount;

                //updating into database
                _db.Coupons.Update(coupon);
                _db.SaveChanges();
                responseDto.Result = _mapper.Map<CouponDto>(coupon);

            }
            catch(Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = ex.Message;
            }
            return responseDto;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public ResponseDto Delete(int id)
        {
            try
            {
                //Check whether coupon is available
                Coupon? coupon = _db.Coupons.FirstOrDefault(u => u.CouponId == id);
                if (coupon is null)
                {
                    responseDto.IsSuccess = false;
                    responseDto.Message = "No Coupon Found";
                    return responseDto;
                }

                //delete from database
                _db.Coupons.Remove(coupon);
                _db.SaveChanges();

            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = ex.Message;
            }
            return responseDto;
        }
    }
}

