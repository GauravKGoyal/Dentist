using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Dentist.Models;
using Dentist.ViewModels;

namespace Dentist
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
        
            Mapper.CreateMap<Paitient, PatientListView>();
            Mapper.CreateMap<Doctor, DoctorListView>();

            Mapper.CreateMap<Practice, SchedulerPracticeView>();
            Mapper.CreateMap<Doctor, SchedulerDoctorView>();
                //.ForMember(d => d.Practices, opt => opt.MapFrom(s => s.Practices));

            Mapper.CreateMap<Paitient, PatientView>()
                .ForMember(d => d.PatientViewPracticeId, opt => opt.MapFrom(s => s.Practice.Id));
            Mapper.CreateMap<PatientView, Paitient>().ForMember(d => d.Practice, opt => opt.Ignore());

            Mapper.CreateMap<Doctor, DoctorView>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id));
            Mapper.CreateMap<DoctorView, Doctor>().ForMember(d => d.Practices, opt => opt.Ignore())
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id));

           Mapper.CreateMap<Practice, int>().ConstructUsing(s => s.Id);


            Mapper.CreateMap<Practice, PracticeView>();
            Mapper.CreateMap<PracticeView, Practice>();

            Mapper.CreateMap<Address, AddressView>();
            Mapper.CreateMap<AddressView, Address>();

            Mapper.CreateMap<Appointment, AppointmentView>()
                .ForMember(d => d.PatientFirstName, opt => opt.MapFrom(s => s.Patient.FirstName))
                .ForMember(d => d.PatientLastName, opt => opt.MapFrom(s => s.Patient.LastName))
                .ForMember(d => d.PatientEmail, opt => opt.MapFrom(s => s.Patient.Email))
                .ForMember(d => d.PatientPhone, opt => opt.MapFrom(s => s.Patient.Phone))
                .ForMember(d => d.DoctorFirstName, opt => opt.MapFrom(s => s.Doctor.FirstName))
                .ForMember(d => d.DoctorLastName, opt => opt.MapFrom(s => s.Doctor.LastName))
                .ForMember(d => d.DoctorEmail, opt => opt.MapFrom(s => s.Doctor.Email))
                .ForMember(d => d.DoctorPhone, opt => opt.MapFrom(s => s.Doctor.Phone))
                .ForMember(d => d.PracticeName, opt => opt.MapFrom(s => s.Practice.Name));
            Mapper.CreateMap<AppointmentView, Appointment>();

            Mapper.CreateMap<DailyAvailability, DailyAvailabilityView>()
                .ForMember(d => d.DailyAvailabilityViewPracticeId, opt => opt.MapFrom(s => s.PracticeId))
                .ForMember(d => d.DailyAvailabilityViewPracticeName, opt => opt.MapFrom(s => s.Practice.Name))
                .ForMember(d => d.DailyAvailabilityViewPersonId, opt => opt.MapFrom(s => s.DoctorId));
            Mapper.CreateMap<DailyAvailabilityView, DailyAvailability>()
                .ForMember(d => d.StartTime1, opt => opt.MapFrom(s => GetHoursMins(s.StartTime1)))
                .ForMember(d => d.StartTime2, opt => opt.MapFrom(s => GetHoursMins(s.StartTime2)))
                .ForMember(d => d.StartTime3, opt => opt.MapFrom(s => GetHoursMins(s.StartTime3)))
                .ForMember(d => d.EndTime1, opt => opt.MapFrom(s => GetHoursMins(s.EndTime1)))
                .ForMember(d => d.EndTime2, opt => opt.MapFrom(s => GetHoursMins(s.EndTime2)))
                .ForMember(d => d.EndTime3, opt => opt.MapFrom(s => GetHoursMins(s.EndTime3)))
                .ForMember(d => d.PracticeId, opt => opt.MapFrom(s => s.DailyAvailabilityViewPracticeId))
                .ForMember(d => d.DoctorId, opt => opt.MapFrom(s => s.DailyAvailabilityViewPersonId));

            Mapper.CreateMap<Appointment, SchedulerAppointmentView>()
                .ForMember(d => d.Start, opt => opt.MapFrom(s => s.StartDateTime))
                .ForMember(d => d.End, opt => opt.MapFrom(s => s.EndDateTime))
                .ForMember(d => d.FirstName, opt => opt.MapFrom(s => s.Patient.FirstName))
                .ForMember(d => d.LastName, opt => opt.MapFrom(s => s.Patient.LastName))
                .ForMember(d => d.Phone, opt => opt.MapFrom(s => s.Patient.Phone))
                .ForMember(d => d.PracticeColor, opt => opt.MapFrom(s => s.Practice.Color));
            Mapper.CreateMap<SchedulerAppointmentView, Appointment>()
            
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