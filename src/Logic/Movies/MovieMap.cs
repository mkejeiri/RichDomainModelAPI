using FluentNHibernate;
using FluentNHibernate.Mapping;

namespace Logic.Movies
{
    public class MovieMap : ClassMap<Movie>
    {
        public MovieMap()
        {
            Id(x => x.Id);
            DiscriminateSubClassesOnColumn("LicensingModel");
            Map(x => x.Name);
            //Map(x => x.LicensingModel).CustomType<int>();
            Map(Reveal.Member<Movie>("LicensingModel")).CustomType<int>();
        }
    }

    //nHibernate won't instatiate the MovieMap but TwoDaysMovieMap or LifeLongMovieMap based on Discriminator 1 or 2
    public class TwoDaysMovieMap : SubclassMap<TwoDaysMovie>
    {
        public TwoDaysMovieMap()
        {
            DiscriminatorValue(1);
        }        
    }

    public class LifeLongMovieMap : SubclassMap<LifeLongMovie>
    {
        public LifeLongMovieMap()
        {
            DiscriminatorValue(2);
        }
    }
}
