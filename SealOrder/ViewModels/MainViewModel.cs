using System.Text.Json;

namespace SealOrder.ViewModels;

public class MainViewModel : ViewModelBase
{
    public MainViewModel()
    {
        title = items[0].ToString();

        left = items[1][0].ToString();

        right = items[2][0].ToString();
    }

    private JsonElement items = JsonDocument.Parse("""
    [
        "生病了",
        [
            "检查",
            [
                "查出来了",
                [
                    "用药",
                    [
                        "好了",
                        "花这么多钱，良心不会痛吗",
                        "true"
                    ],
                    [
                        "没好",
                        "人来的时候好好的，怎么就没了",
                        "true"
                    ]
                ],
                [
                    "不用药",
                    [
                        "好了",
                        "做这么多检查肯定想多收钱",
                        "true"
                    ],
                    [
                        "没好",
                        "庸医杀人，怎么不去死",
                        "true"
                    ]
                ]
            ],
            [
                "没查出来",
                [
                    "用药",
                    [
                        "好了",
                        "没查出来就用药，肯定想骗钱",
                        "true"
                    ],
                    [
                        "没好",
                        "没查出来你用啥药，肯定被你害死的",
                        "true"
                    ]
                ],
                [
                    "不用药",
                    [
                        "好了",
                        "检查做了有啥用，你们这些医生都是骗钱的鬼",
                        "true"
                    ],
                    [
                        "没好",
                        "纳税人的钱白养了你们这群废物，还命！",
                        "true"
                    ]
                ]
            ]
        ],
        [
            "不检查",
            [
                "用药",
                [
                    "好了",
                    "没搞清楚病因就开药，肯定收了回扣",
                    "true"
                ],
                [
                    "没好",
                    "瞎用药，庸医又出来害人了",
                    "true"
                ]
            ],
            [
                "不用药",
                [
                    "好了",
                    "人民好医生",
                    "true"
                ],
                [
                    "没好",
                    "罔顾人命，披着白衣的恶魔",
                    "true"
                ]
            ]
        ]
    ]
    """).RootElement;

    private string title;

    private string left;

    private string right;

    public string Title
    {
        get => title;

        set => this.RaiseAndSetIfChanged(ref title, value);
    }

    public string Left
    {
        get => left;

        set => this.RaiseAndSetIfChanged(ref left, value);
    }

    public string Right
    {
        get => right;

        set => this.RaiseAndSetIfChanged(ref right, value);
    }

    public void LeftClick()
    {
        if (items[2].ToString() != "true")
        {
            items = items[1];

            if (items[2].ToString() != "true")
            {
                Title = items[0].ToString();

                Left = items[1][0].ToString();

                Right = items[2][0].ToString();
            }

            else
            {
                Title = items[1].ToString();

                Left = "赢";

                Right = "赢";
            }
        }
    }

    public void RightClick()
    {
        if (items[2].ToString() != "true")
        {
            items = items[2];

            if (items[2].ToString() != "true")
            {
                Title = items[0].ToString();

                Left = items[1][0].ToString();

                Right = items[2][0].ToString();
            }

            else
            {
                Title = items[2].ToString();

                Left = "赢";

                Right = "赢";
            }
        }
    }
}
