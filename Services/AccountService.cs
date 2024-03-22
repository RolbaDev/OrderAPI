using SimpleProductOrder.Data;
using SimpleProductOrder.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleProductOrder.Services
{
    
    public class AccountService
    {
        private readonly AppDbContext _context;

        public AccountService(AppDbContext context)
        {
            _context = context;
        }

        public bool AccountExists(int id)
        {
            return _context.Accounts.Any(c => c.Id == id);
        }

        public bool CreateAccount(Account account)
        {
            _context.Add(account);
            return Save();
        }

        public bool DeleteAccount(Account account)
        {
            _context.Remove(account);
            return Save();
        }

        public ICollection<Account> GetAccounts()
        {
            return _context.Accounts.ToList();
        }

        public Account GetAccount(int id)
        {
            return _context.Accounts.Where(e => e.Id == id).FirstOrDefault();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateAccount(Account account)
        {
            _context.Update(account);
            return Save();
        }

    }
}
