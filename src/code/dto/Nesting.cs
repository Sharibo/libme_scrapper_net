namespace libme_scrapper.code.dto;

class Nesting {
    public static int Level { get; set; } = 0;

    public static void LevelAdd() => Level += 1;
    public static void LevelRemove() => Level -= 1;
}