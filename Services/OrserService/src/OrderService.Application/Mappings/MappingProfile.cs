using AutoMapper;
using OrderService.Application.Features.Orders.Queries;
using OrderService.Domain.Entities;

namespace OrderService.Application.Mappings
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            // ĐÃ SỬA: Cấu hình ánh xạ chi tiết để tránh lỗi sập Runtime khi khởi chạy ứng dụng
            CreateMap<Order, OrderDto>()
                // Lấy Amount (decimal) từ Value Object Money gán sang TotalAmount của DTO
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount.Amount))

                // Chuyển đổi trạng thái Enum sang chuỗi String để hiển thị cho Client
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}
