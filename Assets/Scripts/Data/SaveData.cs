using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    // Nombre
    public string playerNombre;

    // Correo
    public string playerName;

    /* DATOS PROVISTOS POR EL SERVIDOR */

    public int playerId;


    /* Token: Se utiliza para autenticarse al servidor y autorizar el 
     * envío de runs.
     */
    public string token;

    public int highScore;


    /* LOGROS */
    // runner
    public bool hasRunner;
    public int traveledDistance;

    // rat
    public bool hasRat;
    public int assertionErrors;

    // bunny
    public bool hasBunny;
    public int jumpCount;

    // undamaged
    public bool hasUndamaged;

    // scientist (Doesn't have a specific stat)
    public bool hasScientist;

    public Dictionary<string, List<OutQuestion>> answeredQuestions;
    public Dictionary<string, bool> completedLevels;
}