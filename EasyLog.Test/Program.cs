using ProSoft.EasyLog;

Console.WriteLine("=== Test EasyLog v1.0 (JSON uniquement) ===\n");

// Créer le logger
string logPath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
    "EasySave", "Logs"
);

var logger = new JsonFileLogger(logPath);

Console.WriteLine("Écriture de 3 logs de test...\n");

// Test 1
logger.WriteLog(
    backupName: "Backup_Documents",
    sourceFilePath: @"C:\Users\Yann\Documents\rapport.docx",
    targetFilePath: @"D:\Backups\Documents\rapport.docx",
    fileSize: 2048576,
    transferTime: 1523
);
Console.WriteLine("✅ Log 1 écrit");

// Test 2
logger.WriteLog(
    backupName: "Backup_Photos",
    sourceFilePath: @"C:\Users\Yann\Pictures\photo.jpg",
    targetFilePath: @"\\SERVEUR\Backups\Pictures\photo.jpg",
    fileSize: 524288,
    transferTime: 234
);
Console.WriteLine("✅ Log 2 écrit");

// Test 3 - Erreur
logger.WriteLog(
    backupName: "Backup_Videos",
    sourceFilePath: @"C:\Users\Yann\Videos\film.mp4",
    targetFilePath: @"D:\Backups\Videos\film.mp4",
    fileSize: 0,
    transferTime: -1
);
Console.WriteLine("✅ Log 3 écrit (erreur)\n");

Console.WriteLine($"📄 Fichier créé : {logPath}\\log_{DateTime.Now:yyyy-MM-dd}.json");
Console.WriteLine("\nAppuyez sur une touche...");
Console.ReadKey();
