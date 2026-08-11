namespace Clinic.Application.DTOs.Paging
{
    public class Pager
    {
        public static BasePaging Build(int pageId , int allEntitiesCount , int take , int aroundCurrentPage)
        {
            var pageCount = Convert.ToInt32(Math.Ceiling(allEntitiesCount / (double)take));

            return new BasePaging
            {
                PageId = pageId,
                AllEntitiesCount = allEntitiesCount,
                TakeEntity = take,
                SkipEntity = (pageId - 1) * take,
                StartPage = pageId - aroundCurrentPage <= 0 ? 1 : pageId - aroundCurrentPage,
                EndPage = pageId + aroundCurrentPage > pageCount ? pageCount : pageId + aroundCurrentPage,
                AroundCurrentPage = aroundCurrentPage,
                PageCount = pageCount
            };
        }
    }
}
