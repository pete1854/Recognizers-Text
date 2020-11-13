import json
import os
import time
start = time.time()


def get_input(fname):
    count = 0
    fo = open(fname.split("\\")[-1], "w", encoding="UTF-8")
    notworkfile = open(r".\count\\" + fname.split("\\")[-1], "r", encoding="UTF-8")
    sentences = [sentence.strip() for sentence in notworkfile.readlines()]
    new_list = []
    with open(fname, "r", encoding="UTF-8-SIG") as load_f:
        load_dict = json.load(load_f)
        for i in load_dict:
            if "NotSupported" in i and "dotnet" in i["NotSupported"]:
                if i["Input"] not in sentences:
                    print(i["Input"])
                    count += 1
                    i["NotSupported"] = i["NotSupported"].replace("dotnet", "").strip(",").strip()
            new_list.append(i)
    json.dump(new_list, fo, indent=2, ensure_ascii=False)
    print(fname, count)


def get_all_dir(path):
    filess = os.listdir(path)
    for files in filess:
        if files != "README.md":
            if os.path.isdir(path+files):
                print("dir:"+files)
                get_all_dir(path+files+'\\')
            else:
                if files in ["DateExtractor.json", "DatePeriodExtractor.json", "DatePeriodParser.json", "DateTimePeriodExtractor.json",
                             "DateTimePeriodParser.json", "DurationExtractor.json", "TimePeriodExtractor.json", "TimePeriodParser.json"]:
                    get_input(path+"\\"+files)


path = r".\Specs\DateTime\Spanish\\"
get_all_dir(path)
end = time.time()
print(end-start)
