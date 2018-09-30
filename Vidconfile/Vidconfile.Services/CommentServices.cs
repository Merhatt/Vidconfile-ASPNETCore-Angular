using System;
using System.Collections.Generic;
using System.Text;
using Vidconfile.Data.Contracts;
using Vidconfile.Data.Models;
using Vidconfile.Services.Services;

namespace Vidconfile.Services
{
    public class CommentServices : ICommentServices
    {
        private readonly IRepository<Comment> commentRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepository<VidconfileUser> userRepository;
        private readonly IRepository<Video> videoRepository;

        public CommentServices(IRepository<Comment> commentRepository, IUnitOfWork unitOfWork,
            IRepository<VidconfileUser> userRepository, IRepository<Video> videoRepository)
        {
            this.commentRepository = commentRepository ?? throw new NullReferenceException("commentRepository cannot be null");
            this.unitOfWork = unitOfWork ?? throw new NullReferenceException("unitOfWork cannot be null");
            this.userRepository = userRepository ?? throw new NullReferenceException("userRepository cannot be null");
            this.videoRepository = videoRepository ?? throw new NullReferenceException("videoRepository cannot be null");
        }

        public Comment AddComment(Video video, VidconfileUser author, string commentText)
        {
            if (string.IsNullOrEmpty(commentText))
            {
                throw new NullReferenceException("commentText cannot be null or empty");
            }

            Comment comment = new Comment();

            comment.Video = video ?? throw new NullReferenceException("video cannot be null");
            comment.Author = author ?? throw new NullReferenceException("author cannot be null");
            comment.Created = DateTime.Now;
            comment.CommentText = commentText;

            this.commentRepository.Add(comment);

            this.unitOfWork.Commit();

            return comment;
        }
    }
}
