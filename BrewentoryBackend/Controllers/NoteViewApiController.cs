using Brewentory.Models;
using BrewentoryBackend.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BrewentoryBackend.Controllers
{
    public class NoteViewApiController : ApiController
    {
        // GET: api/noteviewapi
        public string[] GetAll()
        {
            string[] noteArray = null;
            BrewentoryDBEntities1 entities = new BrewentoryDBEntities1();

            try
            {
                noteArray = (from n in entities.Notetables select n.HeadlineID + ", " + n.Headline + ", " + n.Note).ToArray();
            }
            finally
            {
                entities.Dispose();
            }

            return noteArray;
        }

        // GET: api/noteviewapi?headlineID=""
        public BrewentoryModel GetModel(int headlineID)
        {
            BrewentoryDBEntities1 entities = new BrewentoryDBEntities1();
            try
            {
                Notetable nt = (from n in entities.Notetables where (n.HeadlineID == headlineID) select n).FirstOrDefault();
                BrewentoryModel chosenItem = new BrewentoryModel()
                {
                   HeadlineID = nt.HeadlineID,
                   Headline = nt.Headline,
                   Note = nt.Note
                };

                return chosenItem;
            }
            finally
            {
                entities.Dispose();
            }
        }

        [HttpPost]
        public bool PostNote(BrewentoryModel model)
        {
            BrewentoryDBEntities1 entities = new BrewentoryDBEntities1();
            try
            {
                if(model.Operation == "Edit")
                {
                    Notetable existing = (from n in entities.Notetables where (n.HeadlineID == model.HeadlineID) select n).FirstOrDefault();
                    if (existing != null)
                    {
                        existing.Headline = model.Headline;
                        existing.Note = model.Note;
                    }
                    else return false;
                }
                else if(model.Operation == "Delete")
                {
                    Notetable existing = (from n in entities.Notetables where (n.HeadlineID == model.HeadlineID) select n).FirstOrDefault();
                    if (existing != null)
                    {
                        entities.Notetables.Remove(existing);
                    }
                    else return false;
                }
                else if(model.Operation == "Create")
                {
                    Notetable newEntry = new Notetable()
                    {
                        Headline = model.Headline,
                        Note = model.Note
                    };
                    entities.Notetables.Add(newEntry);
                }

                entities.SaveChanges();
            }
            catch
            {
                return false;
            }
            finally
            {
                entities.Dispose();
            }
            return true;
        }

    }
}
