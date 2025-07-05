var LibraryFileSystem = {
  $FileSystem: {
    callback: null,
  },

  FileSystemSyncfsAddEvent: function (cb) {
    FileSystem.callback = cb;
  },

  FileSystemSyncfs: function (id) {
    FS.syncfs(function (err) {
            if (err) {
                console.error("Error syncing filesystem to IndexedDB:", err);
            } else {
                console.log("Filesystem successfully synced to IndexedDB.");
            }
    });
  }
};

autoAddDeps(LibraryFileSystem, '$FileSystem');
mergeInto(LibraryManager.library, LibraryFileSystem);

