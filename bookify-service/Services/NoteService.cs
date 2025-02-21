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
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;

        public NoteService(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public async Task<IEnumerable<NoteModel>> GetAllAsync()
        {
            var notes = await _noteRepository.GetAllAsync();
            return notes.Select(n => new NoteModel
            {
                NoteId = n.NoteId,
                Content = n.Content,
                Status = n.Status,
                CreatedDate = n.CreatedDate,
                LastEdited = n.LastEdited,
                AccountId = n.AccountId
            }).ToList();
        }

        public async Task<NoteModel?> GetByIdAsync(int id)
        {
            var note = await _noteRepository.GetByIdAsync(id);
            if (note == null) return null;

            return new NoteModel
            {
                NoteId = note.NoteId,
                Content = note.Content,
                Status = note.Status,
                CreatedDate = note.CreatedDate,
                LastEdited = note.LastEdited,
                AccountId = note.AccountId
            };
        }

        public async Task<IEnumerable<NoteModel>> GetByAccountIdAsync(int accountId)
        {
            var notes = await _noteRepository.GetByAccountIdAsync(accountId);
            if (notes == null) return null;

            return notes.Select(n => new NoteModel
            {
                NoteId = n.NoteId,
                Content = n.Content,
                Status = n.Status,
                CreatedDate = n.CreatedDate,
                LastEdited = n.LastEdited,
                AccountId = n.AccountId
            }).ToList();
        }

        public async Task<Note> CreateAsync(string? content, int status, int accountId)
        {
            //var existAccount = await _accountRepository.GetByIdAsync(accountId);
            //if (existAccount == null)
            //{
            //    throw new Exception("Account not found.");
            //}

            var note = new Note
            {
                Content = content,
                Status = status,
                CreatedDate = DateTime.UtcNow,
                LastEdited = DateTime.UtcNow,
                AccountId = accountId
            };

            await _noteRepository.AddAsync(note);
            return note;
        }

        public async Task<Note> UpdateAsync(int id, string? content, int status)
        {
            var note = await _noteRepository.GetByIdAsync(id);
            if (note == null)
            {
                throw new Exception("Note not found.");
            }

            note.Content = content;
            note.Status = status;
            note.LastEdited = DateTime.UtcNow;

            await _noteRepository.UpdateAsync(note);
            return note;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var note = await _noteRepository.GetByIdAsync(id);
            if (note == null) return false;

            note.Status = 0;
            note.LastEdited = DateTime.UtcNow;

            await _noteRepository.UpdateAsync(note);
            return true;
        }

        public async Task<bool> ChangeStatus(int id, int status)
        {
            var note = await _noteRepository.GetByIdAsync(id);
            if (note == null) return false;

            note.Status = status;
            note.LastEdited = DateTime.UtcNow;

            await _noteRepository.UpdateAsync(note);
            return true;
        }

        
    }

}
