using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MonthlyBudget.Models;

namespace MonthlyBudget.Services
{
    public interface ICategorySvc
    {
        void Add(Category newCategory);     //adds the category to the database
        List<string> FindAll(string user);  //find all categories for the user
        int Commit();                       //writes the changes to the database
        bool Exists(Category cat);           //checks to see if category already exists for that user
        bool Remove(Category cat);
    }
}
