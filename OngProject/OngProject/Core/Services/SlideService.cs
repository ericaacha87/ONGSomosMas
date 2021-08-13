using Microsoft.AspNetCore.Http;
using OngProject.Core.DTOs;
using OngProject.Core.Helper;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Interfaces.IUnitOfWork;
using OngProject.Core.Mapper;
using OngProject.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace OngProject.Core.Services
{
    public class SlideService : ISlideService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SlideService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Delete(int id) 
        {
            try
            {
               await _unitOfWork.SlideRepository.Delete(id);
               await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }


        public async Task<IEnumerable<SlideDto>> GetAll()
        {
            var mapper = new EntityMapper();
            var slideList = await _unitOfWork.SlideRepository.GetAll();
            var slidesDtoList = slideList.Select(x => mapper.FromSlideToSlideDto(x)).ToList();
            return slidesDtoList;
        }

        public Task<SlideModel> GetById(int Id)
        {
            return _unitOfWork.SlideRepository.GetById(Id);
        }

        public bool EntityExists(int id)
        {
            return _unitOfWork.SlideRepository.EntityExists(id);
        }

        public async Task<SlideModel> Post(SlideDto slideCreateDto)
        {
            var mapper = new EntityMapper();
            var slide = mapper.FromSlideDtoToSlide(slideCreateDto);
            byte[] bytesFile = Convert.FromBase64String(slideCreateDto.ImageUrl);
            ValidateFiles validate = new ValidateFiles();
            string fileExtension = validate.GetImageExtensionFromFile(bytesFile);
            string uniqueName = "slide_" + DateTime.Now.ToString().Replace(",", "").Replace("/", "").Replace(" ", "");
            FormFileData formFileData = new()
            {
                FileName = uniqueName + fileExtension,
                ContentType = "Image/" + fileExtension.Replace(".", ""),
                Name = null
            };
            IFormFile ImageFormFile = ConvertFile.BinaryToFormFile(bytesFile, formFileData);
            S3AwsHelper s3Helper = new();
            var result = await s3Helper.AwsUploadFile(uniqueName, ImageFormFile);
            if (slideCreateDto.Order == 0)
            {
                var slideList = await _unitOfWork.SlideRepository.GetAll();
                var elem = slideList.Last();
                slideCreateDto.Order = elem.Order;
            }
            await _unitOfWork.SlideRepository.Insert(slide);
            await _unitOfWork.SaveChangesAsync();

            return slide;
        }
    }
}
