namespace Webtechshop.Models
{
    public class Paginate
    {
        public int CurrentPage { get; private set; } //trang hiện tại
        public int PageSize { get; private set; } //tổng số mục/trang
        public int TotalItems { get; private set; } // tổng số mục
        public int TotalPages { get; private set; } // tổng số trang
        public int StartPage { get; private set; } // trang bắt đầu
        public int EndPage { get; private set; }    // trang kết thúc
        public Paginate()
        {

        }
        public Paginate(int totalItems, int page, int pageSize = 10)
        {
            int totalPages = (int)Math.Ceiling((decimal)totalItems/(decimal)pageSize);
            int currentPage = page;
            int startPage = currentPage - 5; //trang bắt đầu trừ 5 buttom
            int endPage = currentPage + 4; //trang kết thúc cộng 4 buttom
            if (startPage <= 0)
            {
                endPage = endPage - (startPage - 1); //nếu trang bắt đầu nhỏ hơn hoặc bằng 0 thì gán là 1
                startPage = 1; //trang bắt đầu là 1
            }
            if (endPage > totalPages)
            {
                endPage = totalPages; //nếu trang kết thúc lớn hơn tổng số trang thì gán là tổng số trang
                if (endPage > 10)
                {
                    startPage = endPage - 9; //nếu trang kết thúc trừ 9 lớn hơn 0 thì gán là trang kết thúc trừ 9
                }
            }
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalItems = totalItems;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
        }       
    }
}
