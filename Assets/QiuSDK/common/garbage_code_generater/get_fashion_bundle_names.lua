
function string.split(input, delimiter)
    input = tostring(input)
    delimiter = tostring(delimiter)
    if (delimiter=='') then return false end
    local pos,arr = 0, {}
    -- for each divider found
    for st,sp in function() return string.find(input, delimiter, pos, true) end do
        table.insert(arr, string.sub(input, pos, st - 1))
        pos = sp + 1
    end
    table.insert(arr, string.sub(input, pos))
    return arr
end

function GetBundlePathList(equip_pos, model_id)
	if equip_pos == 1 then
		return {string.format("models/weapon/%d.ab", model_id)}
	elseif equip_pos == 2 then
		return {string.format("models/assist/%d.ab", model_id)}
	elseif equip_pos == 7 then
		return {string.format("models/role/%d.ab", model_id)}
	end
end

function main()

	config = {}
	bundle_list = {}

	package.path = arg[1]

	require("config_fashion")

	arg[2] = string.gsub(arg[2], " ", "")
	local fashion_id_list = string.split(arg[2], ",")

	for i,fashion_id_str in ipairs(fashion_id_list) do
		local fashion_id = tonumber(fashion_id_str)
		local config_with_career = config.fashion[fashion_id]
		for career,conf in pairs(config_with_career) do
			local this_bundle_path_list = GetBundlePathList(conf.equip_pos, conf.model_id)
			for _,bundle_path in ipairs(this_bundle_path_list) do
				table.insert(bundle_list, bundle_path)
			end
		end
	end

	local ret = table.concat( bundle_list, ", ")

	io.write(ret)
end

main()
