using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using DigitalFrame.Core.Extensions;
using DigitalFrame.Service.Core;
using Google.GData.Photos;
using DigitalFrame.Core;
using Google.Contacts;
using Google.GData.Client;
using Google.GData.Contacts;
using System.Diagnostics;

namespace DigitalFrame.Service.PicasaImage {
  public class PicasaImageService : IImageService {

    public event EventHandler ImagesLoaded;

    public event EventHandler<GetImageEventArgs> GetImageComplete;

    private PicasaService service;
    private List<PicasaEntry> allPhotos;

    //private List<PicasaEntry> albums;
    //private int albumIndex;
    //private PicasaFeed currentAlbum;
    //private int photoIndex;

    public PicasaImageService() {
      BeginLoadAlbums();
    }

    private void BeginLoadAlbums() {
      var backgroundWorker = new BackgroundWorker();

      backgroundWorker.DoWork += LoadAlbumBackground;
      backgroundWorker.RunWorkerCompleted += LoadAlbumBackgroundComplete;
      backgroundWorker.RunWorkerAsync();
    }

    private void LoadAlbumBackground(object sender, DoWorkEventArgs e) {
      //albums = new List<PicasaEntry>();
      //albumIndex = -1;

      // Prepare for contact query
      string applicationName = "DigitalFrame";
      string username = ""; // TODO: Load from settings
      string password = "";

      RequestSettings contactRequestSettings = new RequestSettings(applicationName, username, password);
      ContactsRequest contactRequest = new ContactsRequest(contactRequestSettings);

      // Request all groups of the auth user.
      //var allGroups = contactRequest.GetGroups();
      //foreach (var item in allGroups.Entries) {
      //  Debug.Write(item.Id + " - " + item.Title);
      //}

      // Get Family group entry.
      StringBuilder gs = new StringBuilder(GroupsQuery.CreateGroupsUri("default"));
      gs.Append("/");
      gs.Append("e"); // Family
      Feed<Group> gf = contactRequest.Get<Group>(new Uri(gs.ToString()));
      Group familyGroup = gf.Entries.First();

      // Get all contacts in Family group.
      ContactsQuery contactQuery = new ContactsQuery(ContactsQuery.CreateContactsUri("default"));
      contactQuery.Group = familyGroup.Id;
      Feed<Contact> friends = contactRequest.Get<Contact>(contactQuery);

      // Prepare for picasa query
      service = new PicasaService(applicationName);
      service.setUserCredentials(username, password);
      service.SetAuthenticationToken(service.QueryAuthenticationToken());

      allPhotos = new List<PicasaEntry>();

      foreach (var contact in friends.Entries) {
        try {
          string friendUsername = ParseUserName(contact);
          if (!string.IsNullOrEmpty(friendUsername)) {
            AlbumQuery query = new AlbumQuery();
            query.Uri = new Uri(PicasaQuery.CreatePicasaUri(friendUsername));

            PicasaFeed picasaFeed = service.Query(query);
            if (picasaFeed != null && picasaFeed.Entries.Count > 0) {
              foreach (PicasaEntry albumEntry in picasaFeed.Entries) {
                PhotoQuery photosQuery = new PhotoQuery(albumEntry.FeedUri);
                var photosFeed = (PicasaFeed)service.Query(photosQuery);
                foreach (PicasaEntry photoEntry in photosFeed.Entries) {
                  allPhotos.Add(photoEntry);
                }
              }
            }
          }
        }
        catch (LoggedException) {
          // Ignore the exception and continue.
        }
      }
    }

    private string ParseUserName(Contact contact) {
      string username = null;

      foreach (var email in contact.Emails) {
        string address = email.Address.ToLower();
        if (address.EndsWith("@gmail.com")) {
          username = address.Split('@')[0];
          break;
        }
      }

      return username;
    }

    private void LoadAlbumBackgroundComplete(object sender, RunWorkerCompletedEventArgs e) {
      ImagesLoaded.Raise(this, EventArgs.Empty);
    }

    private Random random = new Random();

    public void GetImage() {
      int nextPhotoIndex = random.Next(allPhotos.Count);

      PicasaEntry photoEntry = allPhotos[nextPhotoIndex];
      string photoUri = photoEntry.Media.Content.Url;

      BitmapImage image = new BitmapImage();
      image.BeginInit();
      image.UriSource = new Uri(photoUri);
      image.EndInit();

      GetImageComplete.Raise(this, new GetImageEventArgs { Image = image, Title = photoEntry.Title.Text });
    }


    //public void GetImage() {
    //  bool found = false;
    //  while (!found) {
    //    found = LoadNextPhoto();
    //    if (!found) LoadNextAlbum();
    //  }

    //  PicasaEntry photoEntry = (PicasaEntry) currentAlbum.Entries[photoIndex];
    //  string photoUri = photoEntry.Media.Content.Url;

    //  BitmapImage image = new BitmapImage();
    //  image.BeginInit();
    //  image.UriSource = new Uri(photoUri);
    //  image.EndInit();

    //  GetImageComplete.Raise(this, new EventArgs<BitmapImage> { Payload = image });
    //}

    //private bool LoadNextPhoto() {
    //  if (albumIndex == -1) return false;
    //  if (photoIndex + 1 >= currentAlbum.Entries.Count) return false;

    //  photoIndex++;
    //  return true;
    //}

    //private bool LoadNextAlbum() {
    //  albumIndex = ((albumIndex + 1) % albums.Count);

    //  string albumUri = albums[albumIndex].FeedUri;
    //  if (!string.IsNullOrEmpty(albumUri)) {
    //    var albumQuery = new PhotoQuery(albumUri);
    //    currentAlbum = (PicasaFeed) service.Query(albumQuery);
    //    photoIndex = -1;

    //    return true;
    //  }

    //  return false;
    //}

  }
}
