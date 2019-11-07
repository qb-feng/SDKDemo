print("开始处理lua配置文件");

function GetEngilsh(content)
    -- body
    local result = "";

    if not content then
        return result;
    end

    local isFirst = false;

    for i = 1,string.len(content) - 1 do
        local tempChar = string.sub(content,i, i);
        local tempAscii = string.byte( tempChar);
        if tempAscii and ((tempAscii >= 65 and tempAscii <= 90) or (tempAscii >= 97 and tempAscii <= 122)) then
            if not isFirst then
                isFirst = true;
                tempChar = string.upper(tempChar);
            end
            result = result .. tempChar;
        end
    end

    return result;
end


function HandConfig(configName)

    local resutFileName = configName .. ".lua";
    local sourceFileName = configName;

    local resultFile = io.open(resutFileName,'w');
    file = io.open(sourceFileName,'r');

    local config = {};
    for line in file:lines() do
    -- print(line)
        if line and line ~= nil and line ~= "" and string.len(line) > 0  then

            config[#config+1] = line;
        end
    end

    print("处理结束，数量：",#config);

    resultFile:write("config." .. sourceFileName .." = { \n");

    for i = 1,#config do
        print(config[i]);
        local tempConfig = GetEngilsh(config[i]);
        if tempConfig and tempConfig ~= "" then
            resultFile:write("  \"".. tempConfig .. "\",\n")
        end
        
    end
    resultFile:write("}");

    file:close();

    resultFile:close();
end


--程序开启
local congfigs = {"luatool_config_dongci","luatool_config_mingci","luatool_config_xingrongci"};

for i = 1,#congfigs do
    HandConfig(congfigs[i]);
end




