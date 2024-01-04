using ProsperDaily.MVVM.Views;
using ProsperDaily.Repositories;

namespace ProsperDaily
{


    public partial class App : Application
    {
        public static BaseRepository<MVVM.Models.Transaction>
          TransactionsRepo
        { get; private set; }

        public App(BaseRepository<MVVM.Models.Transaction> _transcationsRepo)
        {
            InitializeComponent();

            TransactionsRepo = _transcationsRepo;

            MainPage = new AppContainer();
        }
    }
}