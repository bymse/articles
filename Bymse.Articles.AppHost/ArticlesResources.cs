namespace Bymse.Articles.AppHost;

public static class ArticlesResources
{
    public const string Apis = "Apis";
    public const string DbMigrator = "DbMigrator";
    public const string Workers = "Workers";
    public const string RabbitMq = "articles-rabbitmq";
    public const string Postgres = "articles-postgres";
    public const string GreenMail = "GreenMail";

    public static readonly string[] Services = [RabbitMq, Postgres, Apis, Workers];
}