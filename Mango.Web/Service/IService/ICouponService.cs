using System;
using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
	public interface ICouponService
	{
		Task<ResponseDto?> GetCouponAsync(String couponCode);
        Task<ResponseDto?> GetCouponByIdAsync(int id);
        Task<ResponseDto?> GetAllCouponsAsync();
        Task<ResponseDto?> CreateNewCouponsync(CouponDto couponDto);
        Task<ResponseDto?> UpdateCouponsync(int id, CouponDto couponDto);
        Task<ResponseDto?> DeleteCouponAsync(int id);

    }
}


