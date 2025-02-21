using bookify_data.Entities;
using bookify_data.Interfaces;
using bookify_data.Model;
using bookify_service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_service.Services
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;

        public NewsService(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<bool> ChangeStatus(int id, int status)
        {
            var news = await _newsRepository.GetByIdAsync(id);
            if (news == null)
            {
                return false;
            }

            news.Status = status;
            news.LastEdited = DateTime.Now;

            await _newsRepository.UpdateAsync(news);
            return true;
        }

        public async Task<News> CreateAsync(string? title, string? content, string? summary, string? imageUrl, DateTime publishAt, int accountId, int status)
        {
            var news = new News
            {
                Title = title,
                Content = content,
                Summary = summary,
                ImageUrl = imageUrl,
                Views = 0,
                PublishAt = publishAt,
                AccountId = accountId,
                Status = status,
                CreatedDate = DateTime.Now,
                LastEdited = DateTime.Now
            };

            await _newsRepository.AddAsync(news);
            return news;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var news = await _newsRepository.GetByIdAsync(id);
            if (news == null)
            {
                return false;
            }

            news.Status = 0;
            news.LastEdited = DateTime.Now;

            await _newsRepository.UpdateAsync(news);
            return true;
        }

        public async Task<IEnumerable<NewsModel>> GetAllAsync()
        {
            var news = await _newsRepository.GetAllAsync();
            return news.Select(news => new NewsModel
            {
                NewsId = news.NewsId,
                Title = news.Title,
                Content = news.Content,
                Summary = news.Summary,
                ImageUrl = news.ImageUrl,
                Views = news.Views,
                PublishAt = news.PublishAt,
                AccountId = news.AccountId,
                Status = news.Status,
                CreatedDate = news.CreatedDate,
                LastEdited = news.LastEdited
            }).ToList();
        }

        public async Task<IEnumerable<NewsModel?>> GetByAccountIdAsync(int accountId)
        {
            var news = await _newsRepository.GetByAccountIdAsync(accountId);
            if (news == null)
            {
                return null;
            }

            return news.Select(news => new NewsModel
            {
                NewsId = news.NewsId,
                Title = news.Title,
                Content = news.Content,
                Summary = news.Summary,
                ImageUrl = news.ImageUrl,
                Views = news.Views,
                PublishAt = news.PublishAt,
                AccountId = news.AccountId,
                Status = news.Status,
                CreatedDate = news.CreatedDate,
                LastEdited = news.LastEdited
            }).ToList();
        }

        public async Task<NewsModel?> GetByIdAsync(int id)
        {
            var news = await _newsRepository.GetByIdAsync(id);
            if (news == null)
            {
                return null;
            }

            return new NewsModel
            {
                NewsId = news.NewsId,
                Title = news.Title,
                Content = news.Content,
                Summary = news.Summary,
                ImageUrl = news.ImageUrl,
                Views = news.Views,
                PublishAt = news.PublishAt,
                AccountId = news.AccountId,
                Status = news.Status,
                CreatedDate = news.CreatedDate,
                LastEdited = news.LastEdited
            };
        }

        public async Task<News> UpdateAsync(int id, string? title, string? content, string? summary, string? imageUrl, DateTime publishAt, int status)
        {
            var news = await _newsRepository.GetByIdAsync(id);
            if (news == null)
            {
                return null;
            }

            news.Title = title;
            news.Content = content;
            news.Summary = summary;
            news.ImageUrl = imageUrl;
            news.PublishAt = publishAt;
            news.Status = status;
            news.LastEdited = DateTime.Now;

            await _newsRepository.UpdateAsync(news);
            return news;
        }
    }
}
