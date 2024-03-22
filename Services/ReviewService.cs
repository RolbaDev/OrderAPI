using SimpleProductOrder.Data;
using SimpleProductOrder.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleProductOrder.Services
{
    public class ReviewService
    {
        private readonly AppDbContext _context;

        public ReviewService(AppDbContext context)
        {
            _context = context;
        }

        public bool ReviewExists(int id)
        {
            return _context.Reviews.Any(c => c.Id == id);
        }

        public bool CreateReview(Review review)
        {
            _context.Add(review);
            return Save();
        }

        public bool DeleteReview(Review review)
        {
            _context.Remove(review);
            return Save();
        }

        public bool DeleteReviews(List<Review> reviews)
        {
            _context.RemoveRange(reviews);
            return Save();
        }

        public ICollection<Review> GetReviews()
        {
            return _context.Reviews.ToList();
        }

        public Review GetReview(int reviewId)
        {
            return _context.Reviews.Where(r => r.Id == reviewId).FirstOrDefault();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateReview(Review review)
        {
            _context.Update(review);
            return Save();
        }

        public ICollection<Review> GetReviewsOfACustomer(int customerId)
        {
            return _context.Reviews.Where(r => r.Author.Id == customerId).ToList();
        }

    }
}
