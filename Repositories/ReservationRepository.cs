using Microsoft.EntityFrameworkCore;
using reservations_api.Data;
using reservations_api.Models.Entities;

namespace reservations_api.Repositories;

public class ReservationRepository : IReservationRepository
{
  private readonly AppDbContext _context;

  public ReservationRepository(AppDbContext context)
  {
    _context = context;
  }

  public async Task<Reservation> AddAsync(Reservation reservation)
  {
    await _context.Reservations.AddAsync(reservation);
    await _context.SaveChangesAsync();
    return reservation;
  }

  public async Task<List<Reservation>> GetByClassroomAndDateAsync(Guid classroomId, DateOnly date)
  {
    return await _context.Reservations
        .AsNoTracking()
        .Where(r => r.ClassroomId == classroomId && r.Date == date)
        .ToListAsync();
  }

  public async Task<List<Reservation>> GetByDateAsync(DateOnly date)
  {
    return await _context.Reservations
        .Where(r => r.Date == date)          
        .Include(r => r.Classroom)            
        .ToListAsync();
  }

  public async Task<bool> ExistsAsync(Guid id)
  {
        return await _context.Reservations.AnyAsync(r => r.Id == id);
  } 

  public async Task DeleteAsync(Guid id)
  {
        var reservation = await _context.Reservations.FindAsync(id);
        
        if (reservation != null)
        {
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
        }
  }



}