using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Dentist.Models;
using Dentist.Models.Doctor;
using Dentist.Models.Patient;
using Dentist.ViewModels;

namespace Dentist
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {

            Mapper.CreateMap<TreatmentPlan, TreatmentPlanDto>();
            Mapper.CreateMap<TreatmentPlanDto, TreatmentPlan>();
            Mapper.CreateMap<TreatmentDto, Treatment>();
            Mapper.CreateMap<Treatment, TreatmentDto>();

            Mapper.CreateMap<PatientNote, PatientNoteDto>();
            Mapper.CreateMap<PatientNoteDto, PatientNote>();
            Mapper.CreateMap<NoteDto, Note>();
            Mapper.CreateMap<Note, NoteDto>();

            Mapper.CreateMap<PatientNote, PatientNoteViewModel>();
            Mapper.CreateMap<PatientNoteViewModel, PatientNote>();

            Mapper.CreateMap<Note, NoteViewModel>();
            Mapper.CreateMap<NoteViewModel, Note>();

            Mapper.CreateMap<DailyAvailabilitySetting, DailyAvailability>();

            Mapper.CreateMap<CalenderSetting, CalenderSettingViewModel>();
            Mapper.CreateMap<CalenderSettingViewModel, CalenderSetting>();

            Mapper.CreateMap<Patient, PatientListViewModel>();
            Mapper.CreateMap<Doctor, DoctorListViewModel>();

            Mapper.CreateMap<Practice, SchedulerPracticeViewModel>();
            Mapper.CreateMap<Doctor, SchedulerDoctorViewModel>();

            Mapper.CreateMap<Person, PersonListViewModel>();

            Mapper.CreateMap<Person, PersonViewModel>();

            Mapper.CreateMap<Patient, PatientViewModel>()
                .ForMember(d => d.PatientViewPracticeId, opt => opt.MapFrom(s => s.Practice.Id));
            Mapper.CreateMap<PatientViewModel, Patient>().ForMember(d => d.Practice, opt => opt.Ignore());

            Mapper.CreateMap<CareService, int>().ConstructUsing(s => s.Id);
            Mapper.CreateMap<Membership, int>().ConstructUsing(s => s.Id);
            Mapper.CreateMap<Specialization, int>().ConstructUsing(s => s.Id);
            Mapper.CreateMap<Doctor, DoctorViewModel>()
                .ForMember(d => d.Address, opt => opt.MapFrom(s => s.Address));
            Mapper.CreateMap<DoctorViewModel, Doctor>()
                .ForMember(d => d.Practices, opt => opt.Ignore())
                .ForMember(d => d.Services, opt => opt.Ignore())
                .ForMember(d => d.Memberships, opt => opt.Ignore())
                .ForMember(d => d.Specializations, opt => opt.Ignore())
                .ForMember(d => d.Registration, opt => opt.MapFrom(s => s.Registration))
                .ForMember(d => d.Address, opt => opt.MapFrom(s => s.Address));

            Mapper.CreateMap<Qualification, QualificationViewModel>();
            Mapper.CreateMap<QualificationViewModel, Qualification>();

            Mapper.CreateMap<Experience, ExperienceViewModel>();
            Mapper.CreateMap<ExperienceViewModel, Experience>();

            Mapper.CreateMap<VitalSign, VitalSignViewModel>();
            Mapper.CreateMap<VitalSignViewModel, VitalSign>();

            Mapper.CreateMap<Award, AwardViewModel>();
            Mapper.CreateMap<AwardViewModel, Award>();

            Mapper.CreateMap<Practice, int>().ConstructUsing(s => s.Id);
            Mapper.CreateMap<Practice, PracticeViewModel>();
            Mapper.CreateMap<PracticeViewModel, Practice>();

            //Mapper.CreateMap<CareService, ServiceViewModel>();
            //Mapper.CreateMap<ServiceViewModel, CareService>();
            Mapper.CreateMap<Registration, RegistrationViewModel>();
            Mapper.CreateMap<RegistrationViewModel, Registration>();

            Mapper.CreateMap<Address, AddressViewModel>();
            Mapper.CreateMap<AddressViewModel, Address>();

            Mapper.CreateMap<Appointment, AppointmentViewModel>()
                .ForMember(d => d.PatientFirstName, opt => opt.MapFrom(s => s.Patient.FirstName))
                .ForMember(d => d.PatientLastName, opt => opt.MapFrom(s => s.Patient.LastName))
                .ForMember(d => d.PatientEmail, opt => opt.MapFrom(s => s.Patient.Email))
                .ForMember(d => d.PatientPhone, opt => opt.MapFrom(s => s.Patient.Phone))
                .ForMember(d => d.DoctorFirstName, opt => opt.MapFrom(s => s.Doctor.FirstName))
                .ForMember(d => d.DoctorLastName, opt => opt.MapFrom(s => s.Doctor.LastName))
                .ForMember(d => d.DoctorEmail, opt => opt.MapFrom(s => s.Doctor.Email))
                .ForMember(d => d.DoctorPhone, opt => opt.MapFrom(s => s.Doctor.Phone))
                .ForMember(d => d.PracticeName, opt => opt.MapFrom(s => s.Practice.Name));
            Mapper.CreateMap<AppointmentViewModel, Appointment>();

            Mapper.CreateMap<DailyAvailability, DailyAvailabilityViewModel>()
                .ForMember(d => d.DailyAvailabilityViewModelPracticeId, opt => opt.MapFrom(s => s.PracticeId))
                .ForMember(d => d.DailyAvailabilityViewModelPracticeName, opt => opt.MapFrom(s => s.Practice.Name))
                .ForMember(d => d.DailyAvailabilityViewModelPersonId, opt => opt.MapFrom(s => s.DoctorId));
            Mapper.CreateMap<DailyAvailabilityViewModel, DailyAvailability>()
                .ForMember(d => d.StartTime1, opt => opt.MapFrom(s => GetHoursMins(s.StartTime1)))
                .ForMember(d => d.StartTime2, opt => opt.MapFrom(s => GetHoursMins(s.StartTime2)))
                .ForMember(d => d.StartTime3, opt => opt.MapFrom(s => GetHoursMins(s.StartTime3)))
                .ForMember(d => d.EndTime1, opt => opt.MapFrom(s => GetHoursMins(s.EndTime1)))
                .ForMember(d => d.EndTime2, opt => opt.MapFrom(s => GetHoursMins(s.EndTime2)))
                .ForMember(d => d.EndTime3, opt => opt.MapFrom(s => GetHoursMins(s.EndTime3)))
                .ForMember(d => d.PracticeId, opt => opt.MapFrom(s => s.DailyAvailabilityViewModelPracticeId))
                .ForMember(d => d.DoctorId, opt => opt.MapFrom(s => s.DailyAvailabilityViewModelPersonId));

            Mapper.CreateMap<DailyAvailabilitySetting, DailyAvailabilitySettingViewModel>();
            Mapper.CreateMap<DailyAvailabilitySettingViewModel, DailyAvailabilitySetting>()
                .ForMember(d => d.StartTime1, opt => opt.MapFrom(s => GetHoursMins(s.StartTime1)))
                .ForMember(d => d.StartTime2, opt => opt.MapFrom(s => GetHoursMins(s.StartTime2)))
                .ForMember(d => d.EndTime1, opt => opt.MapFrom(s => GetHoursMins(s.EndTime1)))
                .ForMember(d => d.EndTime2, opt => opt.MapFrom(s => GetHoursMins(s.EndTime2)));

            Mapper.CreateMap<Appointment, SchedulerAppointmentViewModel>()
                .ForMember(d => d.Start, opt => opt.MapFrom(s => s.StartDateTime))
                .ForMember(d => d.End, opt => opt.MapFrom(s => s.EndDateTime))
                .ForMember(d => d.FirstName, opt => opt.MapFrom(s => s.Patient.FirstName))
                .ForMember(d => d.LastName, opt => opt.MapFrom(s => s.Patient.LastName))
                .ForMember(d => d.Phone, opt => opt.MapFrom(s => s.Patient.Phone))
                .ForMember(d => d.PracticeColor, opt => opt.MapFrom(s => s.Practice.Color));
            Mapper.CreateMap<SchedulerAppointmentViewModel, Appointment>()
                .ForMember(d => d.StartDateTime, opt => opt.MapFrom(s => s.Start))
                .ForMember(d => d.EndDateTime, opt => opt.MapFrom(s => s.End));

        }

        public static DateTime? GetHoursMins(DateTime? dateTime)
        {
            if (dateTime.HasValue)
                return new DateTime(2015, 01, 01, dateTime.Value.Hour, dateTime.Value.Minute, 0, 0);
            
            return null;
        }
    }
}