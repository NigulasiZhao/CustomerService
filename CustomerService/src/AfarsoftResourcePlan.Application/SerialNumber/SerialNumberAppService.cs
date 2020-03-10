using Abp.Application.Services;
using Abp.Domain.Repositories;
using AfarsoftResourcePlan.SerialNumber.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AfarsoftResourcePlan.SerialNumber
{
    public class SerialNumberAppService : ApplicationService, ISerialNumberAppService
    {
        //private readonly IRepository<SerialNumber, Guid> _serialNumberRepository;
        //private readonly IRepository<Organization, Guid> _organizationRepository;
        //public SerialNumberAppService(IRepository<SerialNumber, Guid> repository, IRepository<Organization, Guid> organizationRepository)
        //{
        //    _serialNumberRepository = repository;
        //    _organizationRepository = organizationRepository;
        //}
        ///// <summary>
        ///// 制单时获取单据编码（更新数据库）
        ///// </summary>
        ///// <param name="NumberInput"></param>
        ///// <returns></returns>
        //public async Task<string> GetAndUpdateOrderCode(GetSerialNumberInput NumberInput)
        //{
        //    int SerialNum = 1;
        //    string OrganizationCode = string.Empty;
        //    Organization Model = _organizationRepository.FirstOrDefault(e => e.Id == NumberInput.OrganizationId);
        //    if (Model == null)
        //    {
        //        OrganizationCode = AbpSession.GetOrganizationCode();
        //    }
        //    else
        //    {
        //        OrganizationCode = Model.OrganizationCode;
        //    }
        //    string DocName = OrganizationCode + "-" + Enum.GetName(typeof(OrderType), NumberInput.OrderType);
        //    var list = _serialNumberRepository.GetAll().AsNoTracking()
        //         .Where(x => x.SerialNumberDate == DateTime.Today.ToString("yyyy-MM-dd") && x.OrderName == DocName).ToList();
        //    //List<SerialNumber> list = await _serialNumberRepository.GetAllListAsync(x => x.SerialNumberDate == DateTime.Today.ToString("yyyy-MM-dd") && x.OrderName == DocName).AsNoTracking();
        //    SerialNumber model = new SerialNumber();
        //    if (list.Count > 0)
        //    {
        //        SerialNum = Convert.ToInt32(list[0].DaySerialNumber) + 1;
        //        model.Id = list[0].Id;
        //        model.SerialNumberDate = list[0].SerialNumberDate;
        //        model.DaySerialNumber = SerialNum.ToString();
        //        model.OrderName = list[0].OrderName;
        //        await _serialNumberRepository.UpdateAsync(model);
        //    }
        //    else
        //    {
        //        model.SerialNumberDate = NumberInput.SerialNumberDate.ToString("yyyy-MM-dd");
        //        model.DaySerialNumber = SerialNum.ToString();
        //        model.OrderName = DocName;
        //        await _serialNumberRepository.InsertAsync(model);
        //    }
        //    return DocName + "-" + DateTime.Today.ToString("yyyyMMdd-") + SerialNum.ToString().PadLeft(5, '0');
        //}
        ///// <summary>
        ///// 制单时获取单据编码（不更新数据库）
        ///// </summary>
        ///// <param name="NumberInput"></param>
        ///// <returns></returns>
        //public async Task<string> GetOrderCode(GetSerialNumberInput NumberInput)
        //{
        //    int SerialNumber = 1;
        //    string OrganizationCode = string.Empty;
        //    Organization Model = _organizationRepository.FirstOrDefault(e => e.Id == NumberInput.OrganizationId);
        //    if (Model == null)
        //    {
        //        OrganizationCode = AbpSession.GetOrganizationCode();
        //    }
        //    else
        //    {
        //        OrganizationCode = Model.OrganizationCode;
        //    }
        //    string DocName = OrganizationCode + "-" + Enum.GetName(typeof(OrderType), NumberInput.OrderType);
        //    List<SerialNumber> list = await _serialNumberRepository.GetAllListAsync(x => x.SerialNumberDate == DateTime.Today.ToString("yyyy-MM-dd") && x.OrderName == DocName);
        //    if (list.Count > 0)
        //    {
        //        SerialNumber = Convert.ToInt32(list[0].DaySerialNumber) + 1;
        //    }
        //    return DocName + "-" + DateTime.Today.ToString("yyyyMMdd-") + SerialNumber.ToString().PadLeft(5, '0');
        //}
    }
}
