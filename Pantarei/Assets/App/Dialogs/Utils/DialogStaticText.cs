namespace Texts
{
    public static class TitleTexts
    {
        public static string WELCOME = "Benvenuto nell'App di Pantarei";
        public static string LEAVING = "Stai Lasciando questa applicazione";

        public static string TUTORIAL0 = "Benvenuto nel tutorial";
        public static string TUTORIAL01 = "Continua così";

        public static string TUTORIAL1 = "Guarda ancora la tua mano: QR Scanner";

        public static string TUTORIAL2 = "Guarda ancora la tua mano: File Manager";
        public static string TUTORIAL2_FOLLOW = "Guarda ancora la tua mano: File manager Follow me";

        public static string TUTORIAL3 = "Guarda ancora la tua mano: Dynamic 365";
        
        public static string TUTORIAL4 = "Guarda ancora la tua mano: Exit";

        public static string TUTORIALEND = "Abbiamo finito.";

        public static string DEFAULT_TITLE = "Inserisci un titolo.";

        public static string DEFAULT_ERROR_TITLE = "Ooops! C'è stato un errore";

        public static string QR_OK = "Puoi iniziare";
    }

    public static class ContentTexts
    {
        public static string WELCOME = string.Format("Benvenuto in OPAS, un'applicazione per l'assistenza all'operatore di macchine industriali.\nClicca \"{0}\" per continuare nell'app.\nClicca \"{1}\" per avviare il Tutorial", ButtonTexts.CONTINUE, ButtonTexts.START_TUTORIAL);
        public static string LEAVING_TO_DYNAMIC365 = string.Format("Cliccando \"{0}\" lascerai questa applicazione per aprire Dynamics355.Chiudendo l'app tornerai qui.", ButtonTexts.CONFIRM_ACTION);
        
        public static string TUTORIAL0 = "Qui imparerai i comandi base di questa app.\nGuarda una mano con il palmo rivolto verso di te per aprire il Menu.";
        public static string TUTORIAL01 = string.Format("Benissimo, ricorda che puoi aprire il menù in qualsiasi momento.\nClicca \"{0}\" per continuare.", ButtonTexts.GO_ON);

        public static string TUTORIAL1 = "Clicca il primo tasto del menu per scannerizzare i codici QR.\nRilevato un QR informerò il sistema che stai iniziando a lavorare.";

        public static string TUTORIAL2 = "Il secondo tasto apre il file manager.\nDa qui puoi aprire i file di documentazione di cui hai bisogno.";
        public static string TUTORIAL2_FOLLOW = "Il file explorer ha attiva la funzione di FOLLOWME (segue la tua vista). Puoi disattivare questa funzione cliccando il tasto UNFOLLOW (accanto a quello di chiusura).\nQuando la funzione è attiva è visibile un background blu dietro la finestra dell'explorer.";
        
        public static string TUTORIAL3 = "Il terzo tasto apre l'app Dynamics 365 per l'assistenza remota.\nUsalo per chiamare qualcuno in caso tu abbia bisogno di aiuto.";
        
        public static string TUTORIAL4 = "L'ultimo tanso serve invece per uscire da questa app.\nUsalo quando hai finito di lavorare.";

        public static string TUTORIALEND = string.Format("Clicca \"{0}\" per uscire dal tutorial", ButtonTexts.CONTINUE);

        public static string ERROR_FETCHING_PDF = "Non sono riuscito a caricare il file dal server.\nPer favore controlla la connessione e/o il path richiesto.";

        public static string QR_OK = "Avvio manutenzione confermato.\nPuoi iniziare a lavorare.";

        public static string STATUS_CODE_ERROR = "Errore nella richiesta:";
    }

    public static class ButtonTexts
    {
        public static string CONTINUE_ON_THIS_APP = "Annulla";
        public static string CONTINUE = "Continua";
        public static string START_TUTORIAL = "Tutorial";
        public static string CONFIRM_ACTION = "Conferma";
        public static string GO_ON = "Avanti";
        public static string OK = "Ok";
        public static string QR_ERROR = "QR Scanner\nError";
    }
}
