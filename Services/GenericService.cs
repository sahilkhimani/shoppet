﻿using shoppetApi.Helper;
using shoppetApi.Interfaces;
using shoppetApi.MyUnitOfWork;
using System.Globalization;

namespace shoppetApi.Services
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<T> _genericRepository;

        public GenericService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _genericRepository = _unitOfWork.GenericRepository<T>();
        }

        public async Task<APIResponse<T>> Add(T entity)
        {
            try
            {
                await _genericRepository.Add(entity);
                await _unitOfWork.SaveAsync();
                return APIResponse<T>.CreateResponse(true, MessageHelper.Success(typeof(T).Name, MessageConstants.createdMessage), entity);
            }
            catch (Exception ex) {
                return APIResponse<T>.CreateResponse(false, MessageHelper.Exception(typeof(T).Name, MessageConstants.creatingMessage, ex.Message), null);
            }
        }

        public async Task<APIResponse<T>> Delete(object id)
        {
            try
            {
                var result = await GetById(id);
                if (!result.Success) return result;

                var parsedId = HelperMethods.ParseId(id);
                await _genericRepository.Delete(parsedId!);
                await _unitOfWork.SaveAsync();
                return APIResponse<T>.CreateResponse(true, MessageHelper.Success(typeof(T).Name, MessageConstants.deletedMessage), null);
            }
            catch (Exception ex) {
                return APIResponse<T>.CreateResponse(false, MessageHelper.Exception(typeof(T).Name, MessageConstants.deletingMessage, ex.Message), null);
            }
        }

        public async Task<APIResponse<IEnumerable<T>>> GetAll()
        {
            try
            {
                var result = await _genericRepository.GetAll();
                if (!result.Any()) return APIResponse<IEnumerable<T>>.CreateResponse(false, MessageHelper.NotFound(typeof(T).Name), null);
               
                return APIResponse<IEnumerable<T>>.CreateResponse(true, MessageHelper.Success(typeof(T).Name, MessageConstants.fetchedMessage), result);
            }
            catch (Exception ex)
            {
                return APIResponse<IEnumerable<T>>.CreateResponse(false, MessageHelper.Exception(typeof(T).Name, MessageConstants.fetchingMessage, ex.Message), null);
            }
        }

        public async Task<APIResponse<T>> GetById(object id)
        {
            try
            {
                var parsedId = HelperMethods.ParseId(id);
                if(parsedId == null) return APIResponse<T>.CreateResponse(false, MessageConstants.InvalidId, null);
          
                var result = await _genericRepository.GetById(parsedId);
                if (result == null) return APIResponse<T>.CreateResponse(false, MessageHelper.NotFound(typeof(T).Name), null);
                
                return APIResponse<T>.CreateResponse(true, MessageHelper.Success(typeof(T).Name, MessageConstants.fetchedMessage), result);
            }
            catch (Exception ex)
            {
                return APIResponse<T>.CreateResponse(false, MessageHelper.Exception(typeof(T).Name, MessageConstants.fetchingMessage, ex.Message), null);
            }

        }   

        public async Task<APIResponse<T>> Update(object id, T entity)
        {
            try
            {
                await _genericRepository.Update(id, entity);
                await _unitOfWork.SaveAsync();
                
                return APIResponse<T>.CreateResponse(true, MessageHelper.Success(typeof(T).Name, MessageConstants.updatedMessage), null);
   
            }
            catch (Exception ex)
            {
                return APIResponse<T>.CreateResponse(false, MessageHelper.Exception(typeof(T).Name, MessageConstants.updatingMessage, ex.Message), null);
            }
        }
        public string ApplyTitleCase(string name)
        {
            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            return ti.ToTitleCase(name);
        }
    }
}
